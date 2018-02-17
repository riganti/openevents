using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Common.Configuration;
using OpenEvents.Backend.Common.Filters;
using OpenEvents.Backend.Common.Mappings;
using OpenEvents.Backend.Common.Messaging;
using OpenEvents.Backend.Common.Queries;
using OpenEvents.Backend.Common.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace OpenEvents.Backend.Common
{
    public abstract class CommonStartup
    {

        public CommonStartup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (hostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<CommonStartup>();
            }

            Configuration = builder.Build();
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
            ConfigureQueries(services);
            ConfigureAdditionalServices(services);
            ConfigurePublisherAndSubscriber(services);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info() { Title = "Open Events API", Version = "v1" });
                options.DescribeAllEnumsAsStrings();
            });

            ConfigureMappings();

            services.AddSingleton<AppInitializer>();
        }

        protected virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            var mongoConfiguration = Configuration.GetSection("mongo").Get<MongoConfiguration>();

            services.AddSingleton(provider => new MongoClient(mongoConfiguration.Url));
            services.AddSingleton(provider => provider.GetService<MongoClient>().GetDatabase(mongoConfiguration.DatabaseName));
        }


        protected abstract void ConfigureMongoCollections(IServiceCollection services);

        protected virtual void ConfigureQueries(IServiceCollection services)
        {
            var queryTypes = GetType().Assembly.FindAllOpenGenericImplementations(typeof(IQuery<>));
            foreach (var queryType in queryTypes)
            {
                services.AddTransient(queryType.Implementation);
                
                var factoryType = typeof(Func<>).MakeGenericType(queryType.Implementation);
                var parameter = Expression.Parameter(typeof(IServiceProvider));
                var inner = Expression.Call(parameter, nameof(IServiceProvider.GetService), null, Expression.Constant(queryType.Implementation));
                var body = Expression.Lambda(factoryType, Expression.Convert(inner, queryType.Implementation));
                var factory = Expression.Lambda(body, parameter).Compile();
                services.AddSingleton(factoryType, (Func<IServiceProvider, object>)factory);
            }
        }

        protected abstract void ConfigureFacades(IServiceCollection services);

        protected abstract void ConfigureAdditionalServices(IServiceCollection services);

        private void ConfigurePublisherAndSubscriber(IServiceCollection services)
        {
            var config = Configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            services.AddSingleton(p => config);

            // register management classes
            services.AddSingleton(provider => SdkContext.AzureCredentialsFactory.FromServicePrincipal(config.ClientId, config.ClientSecret, config.TenantId, AzureEnvironment.AzureGlobalCloud));
            services.AddSingleton<ServiceBusProvisioningService>();

            // register infrastructure classes
            var publisherTypes = GetType().Assembly.FindAllOpenGenericImplementations(typeof(IPublisher<>));
            foreach (var publisherType in publisherTypes)
            {
                services.AddSingleton(publisherType.Interface, publisherType.Implementation);
            }

            var subscriberTypes = GetType().Assembly.FindAllOpenGenericImplementations(typeof(ISubscription<>));
            foreach (var subscriberType in subscriberTypes)
            {
                services.AddSingleton(subscriberType.Implementation);
            }

            // register handlers
            var handlerTypes = GetType().Assembly.FindAllOpenGenericImplementations(typeof(IEventHandler<>));
            foreach (var handlerType in handlerTypes)
            {
                services.AddScoped(handlerType.Implementation);
            }
        }
        
        protected virtual void ConfigureMappings()
        {
            Mapper.Initialize(mapper =>
            {
                // find all mappings and register them automatically
                var mappings = GetType().Assembly.FindAllImplementations<IMapping>();
                foreach (var mapping in mappings)
                {
                    var instance = (IMapping) Activator.CreateInstance(mapping);
                    instance.Configure(mapper);
                }
            });
            Mapper.AssertConfigurationIsValid();
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

            RunInitializerTasks(app);
        }

        private async void RunInitializerTasks(IApplicationBuilder app)
        {
            // async void is correct here, we don't want to wait for all the initializations to complete
            var appInitializer = app.ApplicationServices.GetService<AppInitializer>();
            await appInitializer.RunInitializerTasks(app.ApplicationServices, GetType().Assembly, typeof(CommonStartup).Assembly);
        }
    }
}
