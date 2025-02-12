using TagsService.Models;

public interface IRedisCache
{
    Task<IEnumerable<Tag>> GetTagsAsync();
    Task SetTagsAsync(IEnumerable<Tag> tags);
}
