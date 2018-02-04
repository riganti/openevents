using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Data;
using OpenEvents.Backend.Mailing.Common;

namespace OpenEvents.Backend.Mailing.Data
{
    public class MailTemplate : IIdentifiable
    {
        public string Id { get; set; }

        public MailIntent MailIntent { get; set; }

        public string EventId { get; set; }

        public string EventTitle { get; set; }

        public string LanguageCode { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string FromAddress { get; set; }
    }

}
