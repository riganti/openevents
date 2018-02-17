using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Messaging.Contracts;
using OpenEvents.Backend.Mailing.Common;
using OpenEvents.Backend.Mailing.Facades;
using OpenEvents.Backend.Mailing.Model;
using OpenEvents.Backend.Mailing.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Mailing.Messaging
{
    public class SendRegistrationConfirmationHandler : IEventHandler<OrderCreated>
    {
        private readonly IOrdersApi ordersApi;
        private readonly IEventsApi eventsApi;
        private readonly MailerService mailerService;
        private readonly MailTemplatesFacade mailTemplatesFacade;

        public SendRegistrationConfirmationHandler(IOrdersApi ordersApi, IEventsApi eventsApi, MailerService mailerService, MailTemplatesFacade mailTemplatesFacade)
        {
            this.ordersApi = ordersApi;
            this.eventsApi = eventsApi;
            this.mailerService = mailerService;
            this.mailTemplatesFacade = mailTemplatesFacade;
        }

        public async Task ProcessEvent(OrderCreated data)
        {
            // get data
            var eventData = await eventsApi.ApiEventsByIdGetAsync(data.EventId);
            var orderData = await ordersApi.ApiOrdersByOrderIdGetAsync(data.OrderId);

            var allRegistrations = await eventsApi.ApiRegistrationsByEventIdGetAsync(data.EventId);
            var orderRegistrations = allRegistrations.Where(r => r.OrderId == data.OrderId).ToList();

            // prepare template
            var mailData = new RegistrationConfirmationDTO()
            {
                Event = eventData,
                Registrations = orderRegistrations
            };

            // prepare template
            var template = await mailTemplatesFacade.ResolveTemplate(MailIntent.RegistrationConfirmed, orderData.LanguageCode, orderData.EventId);
            var globalTemplate = await mailTemplatesFacade.ResolveTemplate(MailIntent.GlobalTemplate, orderData.LanguageCode, orderData.EventId);

            // send e-mail
            await mailerService.SendMail(orderData.BillingAddress.ContactEmail, template, globalTemplate, mailData);
        }

    }
}
