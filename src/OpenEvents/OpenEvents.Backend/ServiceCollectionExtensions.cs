using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Data;

namespace OpenEvents.Backend
{
    public static class ServiceCollectionExtensions
    {

        public static void AddMongoDbCollections(this IServiceCollection services, string mongoUrl, string mongoDatabaseName)
        {
            services.AddSingleton<MongoClient>(provider => new MongoClient(mongoUrl));
            services.AddSingleton<IMongoDatabase>(provider => provider.GetService<MongoClient>().GetDatabase(mongoDatabaseName));
            services.AddTransient<IMongoCollection<Event>>(provider => provider.GetService<IMongoDatabase>().GetCollection<Event>("events"));
        } 

    }
}
