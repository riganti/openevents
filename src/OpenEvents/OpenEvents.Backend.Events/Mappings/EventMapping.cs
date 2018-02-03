using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Mappings
{
    public class EventMapping : IMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Event, EventDTO>();
            mapper.CreateMap<Event, EventBasicDTO>();
            mapper.CreateMap<EventCancellationPolicy, EventCancellationPolicyDTO>();
            mapper.CreateMap<EventDate, EventDateDTO>();
            mapper.CreateMap<EventPrice, EventPriceDTO>();

            mapper.CreateMap<EventDTO, Event>()
                .ForMember(e => e.Id, m => m.Ignore());
            mapper.CreateMap<EventCancellationPolicyDTO, EventCancellationPolicy>();
            mapper.CreateMap<EventDateDTO, EventDate>();
            mapper.CreateMap<EventPriceDTO, EventPrice>();
        }

    }
}
