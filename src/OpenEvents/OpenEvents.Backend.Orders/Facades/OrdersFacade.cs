using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrdersFacade
    {
        private readonly IMongoCollection<Order> collection;

        public OrdersFacade(IMongoCollection<Order> collection)
        {
            this.collection = collection;
        }


        public async Task<List<OrderDTO>> GetAllByEvent(string eventId)
        {
            var result = await collection.Find(o => o.EventId == eventId).ToListAsync();

            return result
                .Select(Mapper.Map<OrderDTO>)
                .ToList();
        }


    }
}
