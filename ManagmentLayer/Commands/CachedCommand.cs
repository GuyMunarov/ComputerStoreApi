using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentLayer.Commands
{
    public abstract class CachedCommand<T> : ICommand<string , Task<T>>
    {
        private readonly IDistributedCache cache;

        public CachedCommand(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async virtual Task<T> Execute(string key)
        {
            string res= await cache.GetStringAsync(key);
            if (res == null)
            {
                var response = ExecuteCore();
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };

                DistributedCacheEntryOptions redisOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddDays(7))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                var stringData = JsonConvert.SerializeObject(response, settings);
                var dataToCache = Encoding.UTF8.GetBytes(stringData);

                await cache.SetAsync(key, dataToCache, redisOptions);
                return response;
            }

            return  JsonConvert.DeserializeObject<T>(res);
           
        }


        abstract public T ExecuteCore();

       
    }


    public abstract class CachedCommand<T,T2> : ICommand<string,T2, Task<T>>
    {
        private readonly IDistributedCache cache;

        public CachedCommand(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public async virtual Task<T> Execute(string key,T2 val2)
        {
            string res = await cache.GetStringAsync(key);
            if (res == null)
            {
                var response = ExecuteCore(val2);
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };

                DistributedCacheEntryOptions redisOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddDays(7))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                var stringData = JsonConvert.SerializeObject(response, settings);
                var dataToCache = Encoding.UTF8.GetBytes(stringData);

                await cache.SetAsync(key, dataToCache, redisOptions);
                return response;
            }

            return JsonConvert.DeserializeObject<T>(res);

        }


        abstract public T ExecuteCore(T2 val2);


    }
}
