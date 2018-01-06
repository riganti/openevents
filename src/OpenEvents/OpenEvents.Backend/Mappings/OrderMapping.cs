using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OpenEvents.Backend.Data;
using OpenEvents.Backend.Model;

namespace OpenEvents.Backend.Mappings
{
    public class OrderMapping
    {

        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<Order, OrderDTO>();
        }

    }
}