using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Data;
using OpenEvents.Backend.Mailing.Common;

namespace OpenEvents.Backend.Mailing.Model
{
    public class MailTemplateDTO : IIdentifiable
    {
        public string Id { get; set; }

        public MailIntent MailIntent { get; set; }

        public string EventId { get; set; }

        public string EventTitle { get; set; }

        public string LanguageCode { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        [EmailAddress]
        public string FromAddress { get; set; }

    }
}
