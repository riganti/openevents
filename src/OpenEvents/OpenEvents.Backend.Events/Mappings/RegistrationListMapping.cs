using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Model;

namespace OpenEvents.Backend.Events.Mappings
{
    public class RegistrationListMapping : IMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Registration, RegistrationDTO>();
            
            mapper.CreateMap<RegistrationDTO, Registration>()
                .ForMember(e => e.Id, m => m.Ignore());
        }
        
    }
}