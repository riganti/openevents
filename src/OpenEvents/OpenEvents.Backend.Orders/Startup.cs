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
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Orders.Data;
using OpenEvents.Backend.Orders.Facades;
using OpenEvents.Backend.Orders.Services;
using OpenEvents.Client;

namespace OpenEvents.Backend.Orders
{
    public class Startup : CommonStartup
    {
        public Startup(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }

        protected override void ConfigureMongoCollections(IServiceCollection services)
        {
            services.AddSingleton(provider => provider.GetService<IMongoDatabase>().GetCollection<Order>("orders"));
            services.AddSingleton(provider => provider.GetService<IMongoDatabase>().GetCollection<DiscountCode>("discountCodes"));
        }

        protected override void ConfigureFacades(IServiceCollection services)
        {
            services.AddScoped<OrdersFacade>();
            services.AddScoped<OrderCreationFacade>();
            services.AddScoped<OrderPriceCalculationFacade>();
            services.AddScoped<OrderDiscountFacade>();
        }

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
            services.AddSingleton<IVatNumberValidator, DefaultVatNumberValidator>();
            services.AddSingleton<IVatRateProvider, CzechRepublicVatRateProvider>();

            services.AddSingleton<EventsCache>();

            var documentStorageConfiguration = Configuration.GetSection("documentStorage").Get<DocumentStorageConfiguration>();
            services.AddSingleton(new DocumentStorageService(documentStorageConfiguration));

            services.AddSingleton<IEventsApi>(provider => new EventsApi(Configuration.GetValue<string>("api:events")));
        }
    }
}
