using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Facades;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Model;
using OpenEvents.Backend.Events.Queries;

namespace OpenEvents.Backend.Events.Facades
{
    public class EventsFacade : CrudFacadeBase<Event, EventDTO, EventFilterDTO>
    {

        public EventsFacade(IMongoCollection<Event> collection, Func<EventListQuery> queryFactory) : base(collection, queryFactory)
        {
        }


        public async Task<List<EventBasicDTO>> GetAllBasic()
        {
            var result = await collection.AsQueryable().ToListAsync();
            return result
                .Select(Mapper.Map<EventBasicDTO>)
                .ToList();
        }
        
    }
}
