using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders.Messaging
{
    public class EventChangedEventHandler : IEventHandler<EventChanged>
    {
        private readonly ObjectCache<EventDTO> eventsCache;

        public EventChangedEventHandler(ObjectCache<EventDTO> eventsCache)
        {
            this.eventsCache = eventsCache;
        }

        public Task ProcessEvent(EventChanged data)
        {
            eventsCache.InvalidateKey(data.EventId);
            return Task.CompletedTask;
        }
    }
}
