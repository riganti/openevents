using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Model;

namespace OpenEvents.Backend.Orders.Queries
{
    public class OrderListQuery : MongoCollectionQuery<OrderDTO, Order>, IFilteredQuery<OrderDTO, OrderFilterDTO>
    {
        public OrderFilterDTO Filter { get; set; }

        public OrderListQuery(IMongoCollection<Order> collection) : base(collection)
        {
        }

        protected override IQueryable<Order> GetQueryable(IMongoQueryable<Order> collection)
        {
            IQueryable<Order> query = collection;

            if (!string.IsNullOrEmpty(Filter.SearchText))
            {
                query = query.Where(o => o.BillingAddress.Name.Contains(Filter.SearchText) || o.BillingAddress.ContactEmail.Contains(Filter.SearchText));
            }

            if (!string.IsNullOrEmpty(Filter.EventId))
            {
                query = query.Where(o => o.EventId == Filter.EventId);
            }

            return query;
        }

    }
}