using TagsService.Models;

namespace TagsService.ApiClient
{
    public interface IApiClient
    {
        Task<IEnumerable<Tag>> GetTagsFromApiAsync();
    }
}
