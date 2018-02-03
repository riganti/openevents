using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Exceptions;
using OpenEvents.Backend.Orders.Facades;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Services;

namespace OpenEvents.Backend.Orders.Controllers
{
    [Route("/api/orders")]
    public class OrdersController : Controller
    {
        private readonly OrdersFacade ordersFacade;
        private readonly OrderCreationFacade orderCreationFacade;
        private readonly OrderPriceCalculationFacade orderPriceCalculationFacade;
        private readonly EventsCache eventsCache;


        public OrdersController(OrdersFacade ordersFacade, OrderCreationFacade orderCreationFacade, OrderPriceCalculationFacade orderPriceCalculationFacade, EventsCache eventsCache)
        {
            this.ordersFacade = ordersFacade;
            this.orderCreationFacade = orderCreationFacade;
            this.orderPriceCalculationFacade = orderPriceCalculationFacade;
            this.eventsCache = eventsCache;
        }

        [HttpGet]
        public async Task<List<OrderDTO>> GetList([FromQuery] OrderFilterDTO filter)
        {
            return await ordersFacade.GetAll(filter);
        }


        [HttpPost]
        [Route("{eventId}")]
        public async Task<OrderDTO> Create(string eventId, [FromBody] CreateOrderDTO order)
        {
            var eventData = await eventsCache.Get(eventId);
            return await orderCreationFacade.CreateOrder(eventData, order);
        }

        [HttpPost]
        [Route("{eventId}/calculate")]
        public async Task<PriceDataDTO> Calculate(string eventId, [FromBody] CalculateOrderDTO order)
        {
            var eventData = await eventsCache.Get(eventId);
            var price = orderPriceCalculationFacade.CalculatePriceForOrderAndItems(eventData, order);
            return price.TotalPrice;
        }
        
    }
}
