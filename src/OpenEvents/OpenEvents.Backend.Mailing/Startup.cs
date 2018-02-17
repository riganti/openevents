using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Mailing.Data;
using OpenEvents.Backend.Mailing.Facades;
using OpenEvents.Backend.Mailing.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Mailing
{
    public class Startup : CommonStartup 
    {
        public Startup(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }

        protected override void ConfigureMongoCollections(IServiceCollection services)
        {
            services.AddSingleton(provider => provider.GetService<IMongoDatabase>().GetCollection<MailTemplate>("mailTemplates"));
        }

        protected override void ConfigureFacades(IServiceCollection services)
        {
            services.AddTransient<MailTemplatesFacade>();
        }

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
            services.AddSingleton(CreateSmtpClient());

            services.AddSingleton<MailerService>();
            services.AddSingleton<TemplateProcessor>();

            services.AddSingleton<IEventsApi>(provider => new EventsApi(Configuration.GetValue<string>("api:events")));
            services.AddSingleton<IOrdersApi>(provider => new OrdersApi(Configuration.GetValue<string>("api:orders")));
        }

        private SmtpClient CreateSmtpClient()
        {
            var smtpConfiguration = Configuration.GetSection("smtp").Get<SmtpConfiguration>();
            var smtpClient = new SmtpClient();
            if (!string.IsNullOrEmpty(smtpConfiguration.Directory))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtpClient.PickupDirectoryLocation = smtpConfiguration.Directory;
            }
            else
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Host = smtpConfiguration.Host;
                smtpClient.Port = smtpConfiguration.Port;
                smtpClient.Credentials = new NetworkCredential(smtpConfiguration.Username, smtpConfiguration.Password);
            }

            return smtpClient;
        }
    }
}
