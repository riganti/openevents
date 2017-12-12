using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OpenEvents.Backend.Data;

namespace OpenEvents.Backend
{
    public static class DatabaseServiceExtensions
    {

        public static void AddMongoDbCollections(this IServiceCollection services)
        {
            services.AddSingleton<MongoClient>(provider => new MongoClient("mongodb://localhost:27017"));
            services.AddSingleton<IMongoDatabase>(provider => provider.GetService<MongoClient>().GetDatabase("openevents"));
            services.AddTransient<IMongoCollection<Event>>(provider => provider.GetService<IMongoDatabase>().GetCollection<Event>("events"));
        } 

    }
}
