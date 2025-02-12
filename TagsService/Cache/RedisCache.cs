using Newtonsoft.Json;
using StackExchange.Redis;
using TagsService.Models;

public class RedisCache : IRedisCache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisCache> _logger;

    public RedisCache(IConnectionMultiplexer redis, ILogger<RedisCache> logger)
    {
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<IEnumerable<Tag>> GetTagsAsync()
    {
        var data = await _db.StringGetAsync("tags");
        return string.IsNullOrEmpty(data) ? new List<Tag>() : JsonConvert.DeserializeObject<List<Tag>>(data);
    }

    public async Task SetTagsAsync(IEnumerable<Tag> tags)
    {
        await _db.StringSetAsync("tags", JsonConvert.SerializeObject(tags), TimeSpan.FromHours(1));
    }
}
