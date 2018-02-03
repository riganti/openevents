using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenEvents.Backend.Common;
using OpenEvents.Backend.Invoicing.Services;

namespace OpenEvents.Backend.Invoicing
{
    public class Startup : CommonStartup
    {
        public Startup(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }

        protected override void ConfigureMongoCollections(IServiceCollection services)
        {
            
        }

        protected override void ConfigureFacades(IServiceCollection services)
        {
            
        }

        protected override void ConfigureAdditionalServices(IServiceCollection services)
        {
            services.AddSingleton<InvoicingService>();
        }
    }
}
