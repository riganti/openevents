using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Queries
{
    public class EventListQuery : MongoCollectionQuery<EventDTO, Event>, IFilteredQuery<EventDTO, EventFilterDTO>
    {

        public EventFilterDTO Filter { get; set; }

        public EventListQuery(IMongoCollection<Event> collection) : base(collection)
        {
        }

        protected override IQueryable<Event> GetQueryable(IMongoQueryable<Event> collection)
        {
            IQueryable<Event> query = collection;

            if (Filter.EventType == EventType.Free)
            {
                query = query.Where(e => e.Prices.Count == 0);
            }
            else if (Filter.EventType == EventType.Paid)
            {
                query = query.Where(e => e.Prices.Count > 0);
            }

            return query;
        }
    }
}
