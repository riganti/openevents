using OpenEvents.Backend.Mailing.Common;

namespace OpenEvents.Backend.Mailing.Model
{
    public class MailTemplateFilterDTO
    {

        public MailIntent? MailIntent { get; set; }

        public string EventId { get; set; }

        public string LanguageCode { get; set; }
    }
}