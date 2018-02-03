using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task<List<OrderDTO>> GetAll(string searchText = null, string eventId = null)
        {
            IQueryable<Order> query = collection.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(o => o.BillingAddress.Name.Contains(searchText) || o.BillingAddress.ContactEmail.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(eventId))
            {
                query = query.Where(o => o.EventId == eventId);
            }

            var result = await ((IMongoQueryable<Order>)query).ToListAsync();

            return result
                .Select(Mapper.Map<OrderDTO>)
                .ToList();
        }
        

    }
}
