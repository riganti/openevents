using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Mailing.Data;
using OpenEvents.Backend.Mailing.Facades;
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
            services.AddSingleton<IEventsApi>(provider => new EventsApi(Configuration.GetValue<string>("api:events")));
        }
    }
}
