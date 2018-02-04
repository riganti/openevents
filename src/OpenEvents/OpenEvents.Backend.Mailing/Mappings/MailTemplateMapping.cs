using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Mailing.Data;
using OpenEvents.Backend.Mailing.Model;

namespace OpenEvents.Backend.Mailing.Mappings
{
    public class MailTemplateMapping : IMapping
    {
        public void Configure(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<MailTemplate, MailTemplateDTO>();
            mapper.CreateMap<MailTemplateDTO, MailTemplate>()
                .ForMember(s => s.Id, m => m.Ignore());
        }
    }
}
