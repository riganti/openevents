using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public OrderPriceCalculationFacade(IVatRateProvider vatRateProvider, IVatNumberValidator vatNumberValidator, IDateTimeProvider dateTimeProvider)
        {
            this.vatRateProvider = vatRateProvider;
            this.vatNumberValidator = vatNumberValidator;
            this.dateTimeProvider = dateTimeProvider;
        }


        public CalculateOrderResultDTO CalculatePriceForOrderAndItems(EventDTO eventData, CalculateOrderDTO order)
        {
            if (!string.IsNullOrEmpty(order.BillingAddress.VatNumber))
            {
                // validate VAT
                if (!vatNumberValidator.IsValidVat(order.BillingAddress))
                {
                    throw new InvalidVATException();
                }
            }

            // get current VAT rate
            var now = dateTimeProvider.Now;
            var vatRate = vatRateProvider.GetVatRate(now, order.BillingAddress);

            // calculate prices for order items
            var orderItemPrices = new List<PriceDataDTO>();
            foreach (var i in order.OrderItems)
            {
                var eventPrice = eventData.Prices.SingleOrDefault(p => p.BeginDate <= now && now < p.EndDate && p.Sku == i.Sku);
                if (eventPrice == null)
                {
                    throw new InvalidSkuException();
                }

                var orderItemPrice = new PriceDataDTO();
                orderItemPrice.BasePrice = Round(i.Amount * (decimal)eventPrice.Price.Value);
                orderItemPrice.CurrencyCode = eventPrice.CurrencyCode;
                orderItemPrice.Price = Round(orderItemPrice.BasePrice * (1m - orderItemPrice.DiscountPercent / 100m));
                orderItemPrice.VatRate = vatRate;
                orderItemPrice.PriceInclVat = Round(orderItemPrice.Price * orderItemPrice.VatRate);

                orderItemPrices.Add(orderItemPrice);
            }

            if (orderItemPrices.Select(p => p.CurrencyCode).Distinct().Count() > 1)
            {
                throw new OrderItemsMustUseTheSameCurrencyException();
            }

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

        private decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

    }
}
