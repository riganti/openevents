using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Exceptions;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;
using OpenEvents.Backend.Orders.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrderCreationFacade
    {
        private readonly IMongoCollection<Order> collection;
        private readonly Func<OrderNumbersQuery> orderNumbersQuery;
        private readonly OrderPriceCalculationFacade orderPriceCalculationFacade;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IEventsApi eventsApi;

        public OrderCreationFacade(IMongoCollection<Order> collection, Func<OrderNumbersQuery> orderNumbersQuery, OrderPriceCalculationFacade orderPriceCalculationFacade, IDateTimeProvider dateTimeProvider, IEventsApi eventsApi)
        {
            this.collection = collection;
            this.orderNumbersQuery = orderNumbersQuery;
            this.orderPriceCalculationFacade = orderPriceCalculationFacade;
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
            CalculatePrices(eventData, orderData);

            // assign order id
            await GenerateOrderNumber(orderData);

            // save
            await collection.InsertOneAsync(orderData);
            
            // create registrations
            await CreateRegistrations(eventData, order, orderData);

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
            var lastOrder = (await query.Execute()).FirstOrDefault();

            orderData.Id = IncrementOrderNumber(orderData.CreatedDate, lastOrder);
        }

        private string IncrementOrderNumber(DateTime date, string id = "0000000000")
        {
            var number = Convert.ToInt32(id.Substring(4));
            return $"{date:yyyy}{number + 1:000000}";
        }

        private void CalculatePrices(EventDTO eventData, Order orderData)
        {
            var price = orderPriceCalculationFacade.CalculatePriceForOrderAndItems(eventData, Mapper.Map<CalculateOrderDTO>(orderData));
            for (var i = 0; i < orderData.OrderItems.Count; i++)
            {
                orderData.OrderItems[i].Price = Mapper.Map<PriceData>(price.OrderItemPrices[i]);
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
                SkuDescription = eventData.Prices.Single(p => p.Sku == i.Sku).Description,
                OrderId = orderData.Id
            });
            await eventsApi.ApiRegistrationsByEventIdBatchPostAsync(eventData.Id, registrations);
        }
    }
}
