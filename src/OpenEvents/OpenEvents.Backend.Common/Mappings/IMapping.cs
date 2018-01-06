using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace OpenEvents.Backend.Common.Mappings
{
    public interface IMapping
    {

        void Configure(IMapperConfigurationExpression mapper);

    }
}
