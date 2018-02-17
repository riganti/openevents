using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenEvents.Backend.Mailing.Common
{
    public enum MailIntent
    {
        GlobalTemplate = 0,

        RegistrationConfirmed = 1,
        RegistrationCanceled = 2,

        OrderConfirmed = 3,
        OrderChanged = 4,
        OrderCanceled = 5,

        ProformaInvoice = 6,
        Invoice = 7,
        CreditNote = 8,

        EventInfo = 9,
        EventReminder = 10,

        ExternalInvoiceRequest = 11
    }
}
