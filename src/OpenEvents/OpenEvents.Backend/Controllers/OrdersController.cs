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

namespace OpenEvents.Backend.Controllers
{
    [Route("/api/orders")]
    public class OrdersController : Controller
    {
        private readonly IMongoCollection<Event> eventsCollection;
        private readonly IMongoCollection<RegistrationList> registrationListCollection;
        private readonly IMongoCollection<Order> ordersCollection;

        public OrdersController(IMongoCollection<Event> eventsCollection, IMongoCollection<RegistrationList> registrationListCollection, IMongoCollection<Order> ordersCollection)
        {
            this.eventsCollection = eventsCollection;
            this.registrationListCollection = registrationListCollection;
            this.ordersCollection = ordersCollection;
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
        
    }
    
}
