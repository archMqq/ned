using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace NotificationWorker.Cache.Check
{
    public class RedisCacheChecker : RedisCache, ICacheChecker
    {
        public RedisCacheChecker(IConnectionMultiplexer redis) : base(redis) {}
        public async Task<IResult> CheckInstanceAsync(Guid id)
        {
            var db = _redis.GetDatabase();
            var exists = await db.KeyExistsAsync(id.ToString());
            if (!exists)
            {
                return Results.NotFound();
            }

            return Results.Ok();
        }
    }
}
