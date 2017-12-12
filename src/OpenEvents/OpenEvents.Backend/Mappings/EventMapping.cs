using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Mappings
{
    public class EventMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Event, EventDTO>();
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
