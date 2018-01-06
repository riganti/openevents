using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenEvents.Backend.Common.Data;

namespace OpenEvents.Backend.Common.Services
{
    public abstract class ObjectCache<TValue>
    {

        // TODO: cache invalidation

        private readonly ConcurrentDictionary<string, TValue> cache = new ConcurrentDictionary<string, TValue>();

        public async Task<TValue> Get(string id)
        {
            if (cache.TryGetValue(id, out var existingValue))
            {
                return existingValue;
            }
            else
            {
                var newValue = await GetFreshValue(id);
                cache[id] = newValue;
                return newValue;
            }
        }

        protected abstract Task<TValue> GetFreshValue(string id);

        public void InvalidateKey(string id)
        {
            cache.TryRemove(id, out var _);
        }

    }
}
