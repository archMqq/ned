using StackExchange.Redis;

namespace NotificationWorker.Cache
{
    public abstract class RedisCache
    {
        protected readonly IConnectionMultiplexer _redis;
        protected RedisCache(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
    }
}
