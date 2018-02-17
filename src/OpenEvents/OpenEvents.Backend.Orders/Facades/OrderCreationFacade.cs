using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Backend.Orders.Common;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Exceptions;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrderCreationFacade
    {
        private readonly IMongoCollection<Order> collection;
        private readonly Func<OrderNumbersQuery> orderNumbersQuery;
        private readonly OrderPriceCalculationFacade orderPriceCalculationFacade;
        private readonly IPublisher<OrderCreated> orderCreatedPublisher;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventsApi eventsApi;

        public OrderCreationFacade(IMongoCollection<Order> collection, Func<OrderNumbersQuery> orderNumbersQuery, OrderPriceCalculationFacade orderPriceCalculationFacade, IPublisher<OrderCreated> orderCreatedPublisher, IDateTimeProvider dateTimeProvider, IEventsApi eventsApi)
        {
            this.collection = collection;
            this.orderNumbersQuery = orderNumbersQuery;
            this.orderPriceCalculationFacade = orderPriceCalculationFacade;
            this.orderCreatedPublisher = orderCreatedPublisher;
            this.dateTimeProvider = dateTimeProvider;
            this.eventsApi = eventsApi;
        }

        public async Task<OrderDTO> CreateOrder(EventDTO eventData, CreateOrderDTO order)
        {
            // validate
            var now = dateTimeProvider.Now;
            await ValidateEventAvailability(now, order, eventData);
            
            // create the order
            var orderData = Mapper.Map<Order>(order);
            InitializeOrder(eventData, orderData, now);

            // calculate prices
            await CalculatePrices(eventData, orderData, invalidateDiscountCoupon: true);

            // assign order id
            await GenerateOrderNumber(orderData);

            // save
            await collection.InsertOneAsync(orderData);
            
            // create registrations
            await CreateRegistrations(eventData, order, orderData);

            // publish message
            await orderCreatedPublisher.PublishEvent(new OrderCreated() { OrderId = orderData.Id, EventId = orderData.EventId });

            return Mapper.Map<OrderDTO>(orderData);
        }

        private async Task ValidateEventAvailability(DateTime now, CreateOrderDTO order, EventDTO eventData)
        {
            if (now < eventData.RegistrationBeginDate || now > eventData.RegistrationEndDate)
            {
                throw new RegistrationClosedException();
            }

            var count = await eventsApi.ApiRegistrationsByEventIdCountGetAsync(eventData.Id);
            if (count + order.OrderItems.Count > eventData.MaxAttendeeCount)
            {
                throw new RegistrationCapExceededException();
            }
        }

        private void InitializeOrder(EventDTO eventData, Order orderData, DateTime now)
        {
            orderData.EventId = eventData.Id;
            orderData.EventTitle = eventData.Title;
            orderData.CreatedDate = now;
            orderData.ETag = Guid.NewGuid().ToString();
        }

        private async Task GenerateOrderNumber(Order orderData)
        {
            var query = orderNumbersQuery();
            var lastOrderNumber = (await query.Execute()).FirstOrDefault();

            orderData.Id = IncrementOrderNumber(orderData.CreatedDate, lastOrderNumber ?? "0000000000");
        }

        private string IncrementOrderNumber(DateTime date, string id)
        {
            var number = Convert.ToInt32(id.Substring(4));
            return $"{date:yyyy}{number + 1:000000}";
        }

        private async Task CalculatePrices(EventDTO eventData, Order orderData, bool invalidateDiscountCoupon)
        {
            var calculatedOrder = Mapper.Map<CalculateOrderDTO>(orderData);
            var price = await orderPriceCalculationFacade.CalculatePriceForOrderAndItems(eventData, calculatedOrder, invalidateDiscountCoupon);

            // copy item prices
            for (var i = 0; i < orderData.OrderItems.Count; i++)
            {
                orderData.OrderItems[i].Price = Mapper.Map<PriceData>(price.OrderItemPrices[i]);
            }

            // add extra items (discount)
            for (int i = orderData.OrderItems.Count; i < calculatedOrder.OrderItems.Count; i++)
            {
                orderData.OrderItems.Add(new OrderItem()
                {
                    Price = Mapper.Map<PriceData>(price.OrderItemPrices[i]),
                    Sku = calculatedOrder.OrderItems[i].Sku,
                    Amount = calculatedOrder.OrderItems[i].Amount,
                    Type = OrderItemType.Discount
                });
            }

            orderData.TotalPrice = Mapper.Map<PriceData>(price.TotalPrice);
        }

        private async Task CreateRegistrations(EventDTO eventData, CreateOrderDTO order, Order orderData)
        {
            var registrations = order.OrderItems.Select(i => new RegistrationDTO()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Sku = i.Sku,
                SkuDescription = eventData.Prices.Single(p => p.Sku == i.Sku).Title,
                OrderId = orderData.Id
            });
            await eventsApi.ApiRegistrationsByEventIdBatchPostAsync(eventData.Id, registrations);
        }
    }
}
