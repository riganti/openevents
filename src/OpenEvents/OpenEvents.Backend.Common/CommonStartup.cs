using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Filters;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Common.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace OpenEvents.Backend.Common
{
    public abstract class CommonStartup
    {

        public CommonStartup(IHostingEnvironment hostingEnvironment)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelValidationFilterAttribute());
                options.Filters.Add(new HttpResponseMessageFilter());
            });

            services.AddOptions();

            ConfigureDatabaseServices(services);

            services.AddSingleton<IDateTimeProvider, UtcDateTimeProvider>();

            ConfigureMongoCollections(services);
            ConfigureFacades(services);
            ConfigureAdditionalServices(services);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info() { Title = "Open Events API", Version = "v1" });
            });

            ConfigureMappings();
        }

        protected virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            var mongoConfiguration = Configuration.GetSection("mongo").Get<MongoConfiguration>();

            services.AddSingleton(provider => new MongoClient(mongoConfiguration.Url));
            services.AddSingleton(provider => provider.GetService<MongoClient>().GetDatabase(mongoConfiguration.DatabaseName));
        }


        protected abstract void ConfigureMongoCollections(IServiceCollection services);

        protected abstract void ConfigureFacades(IServiceCollection services);

        protected abstract void ConfigureAdditionalServices(IServiceCollection services);


        protected virtual void ConfigureMappings()
        {
            Mapper.Initialize(mapper =>
            {
                // find all mappings and register them automatically
                var mappings = GetType().Assembly.GetExportedTypes()
                    .Where(t => typeof(IMapping).IsAssignableFrom(t))
                    .Where(t => t.IsClass && !t.IsAbstract);
                foreach (var mapping in mappings)
                {
                    var instance = (IMapping) Activator.CreateInstance(mapping);
                    instance.Configure(mapper);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Open Events API");
            });

            app.UseMvc();
        }
    }
}
