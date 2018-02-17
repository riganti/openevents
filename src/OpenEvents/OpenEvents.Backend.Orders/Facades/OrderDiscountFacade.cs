using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Client;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Orders.Exceptions;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrderDiscountFacade
    {
        private readonly IMongoCollection<DiscountCode> discountCodeCollection;

        public OrderDiscountFacade(IMongoCollection<DiscountCode> discountCodeCollection)
        {
            this.discountCodeCollection = discountCodeCollection;
        }
        

        public async Task ApplyDiscount(EventDTO eventData, CalculateOrderDTO order, List<PriceDataDTO> orderItemPrices, bool invalidateDiscountCoupon)
        {
            // get discount code
            var discountCode = await discountCodeCollection.FindByIdAsync(order.DiscountCode);
            if (discountCode == null || discountCode.ExpirationDate < DateTime.UtcNow || discountCode.ClaimedDate != null)
            {
                throw new InvalidDiscountCodeException();
            }
            
            // apply to products
            for (int i = 0; i < order.OrderItems.Count; i++)
            {
                var orderItem = order.OrderItems[i];
                var orderItemPrice = orderItemPrices[i];

                // try to find an applicable rule
                var rule = discountCode.Rules.FirstOrDefault(r => r.ApplicableSku.Contains(orderItem.Sku));
                if (rule != null)
                {
                    // apply the rule
                    var applicableAmount = Math.Min(orderItem.Amount, rule.MaxAmount);
                    var discountPrice = CalculateDiscountPrice(rule, orderItemPrice);

                    // add the discount order line
                    order.OrderItems.Add(new CalculateOrderItemDTO()
                    {
                        Sku = Constants.DiscountSku,
                        Amount = applicableAmount
                    });
                    orderItemPrices.Add(new PriceDataDTO()
                    {
                        BasePrice = -discountPrice,
                        CurrencyCode = orderItemPrice.CurrencyCode,
                        Price = OrderPriceCalculationFacade.Round(-discountPrice * applicableAmount)
                    });

                    if (invalidateDiscountCoupon)
                    {
                        // claim discount code
                        await InvalidateDiscountCode(order);
                    }

                    return;
                }
            }

            throw new InvalidDiscountCodeException();
        }

        private decimal CalculateDiscountPrice(DiscountCodeRule rule, PriceDataDTO orderItemPrice)
        {
            decimal discountPrice = 0;
            if (rule.DiscountAmount != null)
            {
                discountPrice = rule.DiscountAmount.Value;
            }
            else if (rule.DiscountPercent != null)
            {
                discountPrice = OrderPriceCalculationFacade.Round(orderItemPrice.BasePrice * rule.DiscountPercent.Value / 100m);
            }

            return discountPrice;
        }

        private async Task InvalidateDiscountCode(CalculateOrderDTO order)
        {
            var result = await discountCodeCollection.FindOneAndUpdateAsync(
                c => c.Id == order.DiscountCode && c.ClaimedDate == null,
                Builders<DiscountCode>.Update.Set(c => c.ClaimedDate, DateTime.UtcNow));

            if (result == null)
            {
                throw new InvalidDiscountCodeException();
            }
        }
    }
}
