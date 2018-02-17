using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Backend.Orders.Exceptions;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrderPriceCalculationFacade
    {
        private readonly IVatRateProvider vatRateProvider;
        private readonly IVatNumberValidator vatNumberValidator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly OrderDiscountFacade orderDiscountFacade;

        public OrderPriceCalculationFacade(IVatRateProvider vatRateProvider, IVatNumberValidator vatNumberValidator, IDateTimeProvider dateTimeProvider, OrderDiscountFacade orderDiscountFacade)
        {
            this.vatRateProvider = vatRateProvider;
            this.vatNumberValidator = vatNumberValidator;
            this.dateTimeProvider = dateTimeProvider;
            this.orderDiscountFacade = orderDiscountFacade;
        }


        public async Task<CalculateOrderResultDTO> CalculatePriceForOrderAndItems(EventDTO eventData, CalculateOrderDTO order, bool invalidateDiscountCoupon = false)
        {
            var now = dateTimeProvider.Now;
            
            // calculate prices for order items
            var orderItemPrices = CalculateItemPrices(eventData, order, now);

            // validate there is only one currency
            if (orderItemPrices.Select(p => p.CurrencyCode).Distinct().Count() > 1)
            {
                throw new OrderItemsMustUseTheSameCurrencyException();
            }

            // apply discount
            if (!string.IsNullOrEmpty(order.DiscountCode))
            {
                await orderDiscountFacade.ApplyDiscount(eventData, order, orderItemPrices, invalidateDiscountCoupon);
            }

            // get current VAT rate
            var vatRate = DetermineVat(order, now);

            // apply VAT
            ApplyVat(orderItemPrices, vatRate);

            // calculate total for order
            var totalPrice = new PriceDataDTO()
            {
                BasePrice = Round(orderItemPrices.Sum(p => p.BasePrice)),
                Price = Round(orderItemPrices.Sum(p => p.Price)),
                VatRate = vatRate,
                PriceInclVat = Round(orderItemPrices.Sum(p => p.PriceInclVat)),
                CurrencyCode = orderItemPrices.First().CurrencyCode
            };

            return new CalculateOrderResultDTO()
            {
                TotalPrice = totalPrice,
                OrderItemPrices = orderItemPrices
            };
        }

        private List<PriceDataDTO> CalculateItemPrices(EventDTO eventData, CalculateOrderDTO order, DateTime now)
        {
            var orderItemPrices = new List<PriceDataDTO>();
            foreach (var i in order.OrderItems)
            {
                var eventPrice = eventData.Prices.SingleOrDefault(p => p.BeginDate <= now && now < p.EndDate && p.Sku == i.Sku);
                if (eventPrice == null)
                {
                    throw new InvalidSkuException();
                }

                var orderItemPrice = new PriceDataDTO();
                orderItemPrice.BasePrice = (decimal) eventPrice.Price.Value;
                orderItemPrice.CurrencyCode = eventPrice.CurrencyCode;
                orderItemPrice.Price = Round(i.Amount * orderItemPrice.BasePrice);

                orderItemPrices.Add(orderItemPrice);
            }

            return orderItemPrices;
        }

        private void ApplyVat(List<PriceDataDTO> orderItemPrices, decimal vatRate)
        {
            foreach (var orderItemPrice in orderItemPrices)
            {
                orderItemPrice.VatRate = vatRate;
                orderItemPrice.PriceInclVat = Round(orderItemPrice.Price * orderItemPrice.VatRate);
            }
        }

        private decimal DetermineVat(CalculateOrderDTO order, DateTime now)
        {
            if (!string.IsNullOrEmpty(order.BillingAddress.VatNumber))
            {
                // validate VAT
                if (!vatNumberValidator.IsValidVat(order.BillingAddress))
                {
                    throw new InvalidVATException();
                }
            }

            var vatRate = vatRateProvider.GetVatRate(now, order.BillingAddress);
            return vatRate;
        }

        public static decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

    }
}
