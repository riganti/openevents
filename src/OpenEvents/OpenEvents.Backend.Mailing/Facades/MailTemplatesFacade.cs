using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Facades;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Common.Services;
using OpenEvents.Backend.Mailing.Common;
using OpenEvents.Backend.Mailing.Data;
using OpenEvents.Backend.Mailing.Model;
using OpenEvents.Backend.Mailing.Queries;
using OpenEvents.Client;

namespace OpenEvents.Backend.Mailing.Facades
{
    public class MailTemplatesFacade : CrudFacadeBase<MailTemplate, MailTemplateDTO, MailTemplateFilterDTO>
    {
        private readonly IEventsApi eventsApi;

        public MailTemplatesFacade(IMongoCollection<MailTemplate> collection, Func<MailTemplateListQuery> queryFactory, IEventsApi eventsApi) : base(collection, queryFactory)
        {
            this.eventsApi = eventsApi;
        }


        public async Task<MailTemplateDTO> ResolveTemplate(MailIntent mailIntent, string languageCode, string eventId)
        {
            var templates = await GetAll(new MailTemplateFilterDTO() { MailIntent = mailIntent });

            var match = templates.FirstOrDefault(t => t.LanguageCode == languageCode && t.EventId == eventId);
            if (match != null)
            {
                return match;
            }

            match = templates.FirstOrDefault(t => t.LanguageCode == languageCode);
            if (match != null)
            {
                return match;
            }

            return templates.FirstOrDefault();
        }

        protected override async Task OnInsertingAsync(MailTemplateDTO data, MailTemplate entity)
        {
            await FillEventTitle(entity);
            await base.OnInsertingAsync(data, entity);
        }

        protected override async Task OnUpdatingAsync(MailTemplateDTO data, MailTemplate entity)
        {
            await FillEventTitle(entity);
            await base.OnUpdatingAsync(data, entity);
        }

        private async Task FillEventTitle(MailTemplate data)
        {
            if (!string.IsNullOrEmpty(data.EventId))
            {
                data.EventTitle = (await eventsApi.ApiEventsByIdGetAsync(data.EventId)).Title;
            }
            else
            {
                data.EventTitle = "";
            }
        }
    }
}
