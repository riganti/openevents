using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Events.Data;
using OpenEvents.Backend.Events.Facades;

namespace OpenEvents.Backend.Events
{
    public class Startup : CommonStartup
    {
        public Startup(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }

        protected override void ConfigureMongoCollections(IServiceCollection services)
        {
            services.AddSingleton(provider => provider.GetService<IMongoDatabase>().GetCollection<Event>("events"));
            services.AddSingleton(provider => provider.GetService<IMongoDatabase>().GetCollection<RegistrationList>("registrationLists"));
        }

        protected override void ConfigureFacades(IServiceCollection services)
        {
            services.AddScoped<EventsFacade>();
        }

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
        }
    }
}
