using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Orders.Data;

namespace OpenEvents.Backend.Orders.Queries
{
    public class OrderNumbersQuery : MongoCollectionQuery<string, Order>
    {
        public OrderNumbersQuery(IMongoCollection<Order> collection) : base(collection)
        {
            SortExpressions.Add((o => o.Id, true));
            Take = 1;
        }

        protected override IQueryable<Order> GetQueryable(IMongoQueryable<Order> collection)
        {
            return collection.Where(i => i.ReplacedByOrderId == null);
        }

        protected override string PostProcessResult(Order item)
        {
            return item.Id;
        }
    }
}
