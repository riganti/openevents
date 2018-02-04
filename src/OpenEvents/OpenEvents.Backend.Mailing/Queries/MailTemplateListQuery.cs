using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Mailing.Data;
using OpenEvents.Backend.Mailing.Model;

namespace OpenEvents.Backend.Mailing.Queries
{
    public class MailTemplateListQuery : MongoCollectionQuery<MailTemplateDTO, MailTemplate>, IFilteredQuery<MailTemplateDTO, MailTemplateFilterDTO>
    {
        public MailTemplateFilterDTO Filter { get; set; }

        public MailTemplateListQuery(IMongoCollection<MailTemplate> collection) : base(collection)
        {
        }

        protected override IQueryable<MailTemplate> GetQueryable(IMongoQueryable<MailTemplate> collection)
        {
            IQueryable<MailTemplate> query = collection;

            if (Filter.MailIntent != null)
            {
                query = query.Where(t => t.MailIntent == Filter.MailIntent);
            }
            if (!string.IsNullOrEmpty(Filter.LanguageCode))
            {
                query = query.Where(t => t.LanguageCode == Filter.LanguageCode);
            }
            if (!string.IsNullOrEmpty(Filter.EventId))
            {
                query = query.Where(t => t.EventId == Filter.EventId);
            }

            return query;
        }
    }
}
