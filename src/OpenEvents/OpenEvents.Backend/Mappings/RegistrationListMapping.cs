using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Mappings
{
    public class RegistrationListMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Registration, RegistrationDTO>();
            
            mapper.CreateMap<RegistrationDTO, Registration>()
                .ForMember(e => e.Id, m => m.Ignore());
        }

    }
}