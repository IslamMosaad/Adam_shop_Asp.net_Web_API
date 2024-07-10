
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace EcommerceAPI.Services.cacheServices
{
    public class RedisCache<T>:IRedisCache<T> where T : class
    {
        private readonly IDatabase redisDB;
        public RedisCache(IDistributedCache _redisDB)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            redisDB = redis.GetDatabase();
        }

        public async Task<T> getByIdAsync(int id)
        {
            string key = id.ToString();
            string stringEntity = await redisDB.StringGetAsync(key);
            T entity = null;
            if (stringEntity != null)
            {
                 entity = JsonSerializer.Deserialize<T>(stringEntity);
                return entity;
            }

            // var entity = await repo.GetByIdAsync(id);
            //if (entity != null)
            //{
            //    var stEntity = JsonSerializer.Serialize<T>(entity);

            //    await redisDB.StringSetAsync(key, stEntity, TimeSpan.FromMinutes(5));
            //}
            return entity;
        }

        public async Task<T> setByIdAsync(int id, T entity)
        {
            string key = id.ToString();
            if (entity != null)
            {
                var stEntity = JsonSerializer.Serialize<T>(entity);

                await redisDB.StringSetAsync(key, stEntity, TimeSpan.FromMinutes(30));
            }
            return entity;
        }
    }
}
