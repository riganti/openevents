using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Facades;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Facades
{
    public class EventsFacade : CrudFacadeBase<Event, EventDTO>
    {

        public EventsFacade(IMongoCollection<Event> collection) : base(collection)
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
