using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using OpenEvents.Backend.Data;

namespace OpenEvents.Backend
{
    public static class MongoCollectionExtensions
    {

        public static async Task<T> FindByIdAsync<T>(this IMongoCollection<T> collection, string id) where T : IIdentifiable
        {
            return (await collection.FindAsync(i => i.Id == id)).FirstOrDefault();
        }


        public static async Task<T> ChangeOneSafeAsync<T>(this IMongoCollection<T> collection, string id, Action<T> changeAction) where T : IIdentifiable, IVersionedEntity, new()
        {
            var exceptions = new List<Exception>();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    // load
                    var item = await collection.FindByIdAsync(id);
                    var isNew = false;
                    if (item == null)
                    {
                        item = new T();
                        isNew = true;
                    }

                    var etag = item.ETag;

                    // change
                    changeAction(item);

                    // store
                    item.ETag = Guid.NewGuid().ToString();

                    if (!isNew)
                    {
                        var result = await collection.ReplaceOneAsync(x => x.Id == id && x.ETag == etag, item);
                        if (result.ModifiedCount == 1)
                        {
                            return item;
                        }
                    }
                    else
                    {
                        await collection.InsertOneAsync(item);
                        return item;
                    }
                }
                catch (MongoException ex)
                {
                    exceptions.Add(ex);
                }

                // wait and retry
                await Task.Delay(50);
            }

            throw new AggregateException("Couldn't perform the operation even after 5 retries!", exceptions);
        }

    }
}