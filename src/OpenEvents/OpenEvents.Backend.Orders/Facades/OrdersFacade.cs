using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common.Facades;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;
using OpenEvents.Backend.Orders.Queries;

namespace OpenEvents.Backend.Orders.Facades
{
    public class OrdersFacade : CrudFacadeBase<Order, OrderDTO, OrderFilterDTO>
    {
        
        public OrdersFacade(IMongoCollection<Order> collection, Func<OrderListQuery> queryFactory) : base(collection, queryFactory)
        {
        }

    }
}
