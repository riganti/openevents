using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;
using OpenEvents.Backend.Services;

namespace OpenEvents.Backend.Controllers
{
    [Route("/api/orders")]
    public class OrdersController : Controller
    {
        private readonly IMongoCollection<Event> eventsCollection;
        private readonly IMongoCollection<RegistrationList> registrationListCollection;
        private readonly IMongoCollection<Order> ordersCollection;
        private readonly IVatValidator vatValidator;
        private readonly IVatRateProvider vatRateProvider;

        public OrdersController(IMongoCollection<Event> eventsCollection, IMongoCollection<RegistrationList> registrationListCollection, IMongoCollection<Order> ordersCollection, IVatValidator vatValidator, IVatRateProvider vatRateProvider)
        {
            this.eventsCollection = eventsCollection;
            this.registrationListCollection = registrationListCollection;
            this.ordersCollection = ordersCollection;
            this.vatValidator = vatValidator;
            this.vatRateProvider = vatRateProvider;
        }

        [HttpGet]
        [Route("{eventId}")]
        public List<OrderDTO> GetList(string eventId)
        {
            return ordersCollection.AsQueryable()
                .Where(o => o.EventId == eventId)
                .ToList()
                .Select(Mapper.Map<OrderDTO>)
                .ToList();
        }

        [HttpPost]
        [Route("{eventId}")]
        public async Task<OrderDTO> Create(string eventId, CreateOrderDTO order)
        {
            var now = DateTime.UtcNow;
            
            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (now < e.RegistrationBeginDate || e.RegistrationEndDate < now)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Registration is closed!");
            }
            
            
            // create the order
            var data = Mapper.Map<Order>(order);
            data.EventId = eventId;
            data.CreatedDate = now;
            data.ETag = Guid.NewGuid().ToString();
            
            // assign order id
            var lastOrder = ordersCollection.AsQueryable().OrderByDescending(o => o.Id).FirstOrDefault();
            data.Id = IncrementOrderNumber(data.CreatedDate, lastOrder?.Id);

            // calculate prices
            var (total, itemPrices) = CalculatePrice(now, e, Mapper.Map<CalculateOrderDTO>(data));
            for (var i = 0; i < data.OrderItems.Count; i++)
            {
                data.OrderItems[i].Price = itemPrices[i];
            }
            data.TotalPrice = total;
            await ordersCollection.InsertOneAsync(data);


            // create registrations
            var registrations = order.OrderItems.Select(i => new Registration()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = i.FirstName,
                LastName = i.LastName,
                Email = i.Email,
                Sku = i.Sku,
                SkuDescription = e.Prices.Single(p => p.Sku == i.Sku).Description,
                OrderId = data.Id
            });
            await registrationListCollection.ChangeOneSafeAsync(eventId, list =>
            {
                list.Registrations.AddRange(registrations);
            });

            return Mapper.Map<OrderDTO>(data);
        }

        [HttpPost]
        [Route("{eventId}/calculate")]
        public async Task<PriceDataDTO> Calculate(string eventId, CalculateOrderDTO order)
        {
            var now = DateTime.UtcNow;

            var e = await eventsCollection.FindByIdAsync(eventId);
            if (e == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (now < e.RegistrationBeginDate || e.RegistrationEndDate < now)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Registration is closed!");
            }

            var (total, _) = CalculatePrice(now, e, order);
            return Mapper.Map<PriceDataDTO>(total);
        }
        

        private string IncrementOrderNumber(DateTime date, string id = "0000000000")
        {
            var number = Convert.ToInt32(id.Substring(4));
            return $"{date:yyyy}{number + 1:000000}";
        }

        private (PriceData, List<PriceData>) CalculatePrice(DateTime now, Event e, CalculateOrderDTO order)
        {
            // validate VAT
            if (!vatValidator.IsValidVat(order.Address))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "VAT is not valid!");
            }

            // get VAT
            var vatRate = vatRateProvider.GetVatRate(order.Address);

            // get prices for order items
            var orderItemPrices = new List<PriceData>();
            foreach (var i in order.OrderItems)
            {
                var eventPrice = e.Prices.Single(p => p.BeginDate <= now && now < p.EndDate && p.Sku == i.Sku);

                var orderItemPrice = new PriceData();
                orderItemPrice.BasePrice = Round(i.Amount * eventPrice.Price);
                orderItemPrice.CurrencyCode = eventPrice.CurrencyCode;
                orderItemPrice.Price = Round(orderItemPrice.BasePrice * (1m - orderItemPrice.DiscountPercent / 100m));
                orderItemPrice.VatRate = vatRate;
                orderItemPrice.PriceInclVat = Round(orderItemPrice.Price * orderItemPrice.VatRate);

                orderItemPrices.Add(orderItemPrice);
            }

            // calculate total
            var totalPrice = new PriceData()
            {
                BasePrice = Round(orderItemPrices.Sum(p => p.BasePrice)),
                Price = Round(orderItemPrices.Sum(p => p.Price)),
                VatRate = vatRate,
                PriceInclVat = Round(orderItemPrices.Sum(p => p.PriceInclVat))
            };

            return (totalPrice, orderItemPrices);
        }
    
        private decimal Round(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
