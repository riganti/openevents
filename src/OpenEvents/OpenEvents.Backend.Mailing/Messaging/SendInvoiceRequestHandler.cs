using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;
using OpenEvents.Backend.Mailing.Common;
using OpenEvents.Backend.Mailing.Facades;
using OpenEvents.Backend.Mailing.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Mailing.Messaging
{
    public class SendInvoiceRequestHandler : IEventHandler<OrderCreated>
    {
        private readonly IOrdersApi ordersApi;
        private readonly MailTemplatesFacade mailTemplatesFacade;
        private readonly MailerService mailerService;

        public SendInvoiceRequestHandler(IOrdersApi ordersApi, MailTemplatesFacade mailTemplatesFacade, MailerService mailerService)
        {
            this.ordersApi = ordersApi;
            this.mailTemplatesFacade = mailTemplatesFacade;
            this.mailerService = mailerService;
        }

        public async Task ProcessEvent(OrderCreated data)
        {
            // get data
            var orderData = await ordersApi.ApiOrdersByOrderIdGetAsync(data.OrderId);
            
            // prepare template
            var template = await mailTemplatesFacade.ResolveTemplate(MailIntent.ExternalInvoiceRequest, orderData.LanguageCode, orderData.EventId);
            var globalTemplate = await mailTemplatesFacade.ResolveTemplate(MailIntent.GlobalTemplate, orderData.LanguageCode, orderData.EventId);

            // send e-mail
            await mailerService.SendMail(orderData.BillingAddress.ContactEmail, template, globalTemplate, orderData);
        }

    }
}