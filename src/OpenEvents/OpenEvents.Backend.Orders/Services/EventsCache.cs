using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders.Services
{
    public class EventsCache : ObjectCache<EventDTO>
    {
        private readonly EventsApi eventsApi;

        public EventsCache(EventsApi eventsApi)
        {
            this.eventsApi = eventsApi;
        }

        protected override async Task<EventDTO> GetFreshValue(string id)
        {
            return await eventsApi.ApiEventsByIdGetAsync(id);
        }
    }
}
