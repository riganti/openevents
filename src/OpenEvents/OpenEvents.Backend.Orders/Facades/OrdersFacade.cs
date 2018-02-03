using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrdersFacade
    {
        private readonly IMongoCollection<Order> collection;
        private readonly Func<OrderListQuery> queryFactory;

        public OrdersFacade(IMongoCollection<Order> collection, Func<OrderListQuery> queryFactory)
        {
            this.collection = collection;
            this.queryFactory = queryFactory;
        }

        public async Task<List<OrderDTO>> GetAll(OrderFilterDTO filter)
        {
            var query = queryFactory();
            query.Filter = filter;
            return (await query.Execute()).ToList();
        }
        

    }
}
