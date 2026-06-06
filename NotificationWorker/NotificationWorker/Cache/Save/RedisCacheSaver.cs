using StackExchange.Redis;

namespace NotificationWorker.Cache.Save
{
    public class RedisCacheSaver : RedisCache, ICacheSaver
    {
        readonly Expiration _exp;
        public RedisCacheSaver(IConnectionMultiplexer _redis, Expiration exp) : base(_redis) 
        {
            _exp = exp;
        }

        public async Task<IResult> SaveInstanceAsync(Guid id, string data)
        {
            var db = _redis.GetDatabase();

            await db.StringSetAsync(id.ToString(), data, _exp);

            return Results.Ok();
        }
    }
}
