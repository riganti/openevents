using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Client;

namespace OpenEvents.Admin.Model
{
    public class MailTemplateFilterDTO
    {

        public MailIntent? Intent { get; set; }

        public string EventId { get; set; }

        public string LanguageCode { get; set; }

    }
}
