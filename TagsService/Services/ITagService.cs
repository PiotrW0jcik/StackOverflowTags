using TagsService.Models;

namespace TagsService.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsAsync(int page, int pageSize, string sortBy, bool descending);
        Task RefreshTagsAsync();
    }
}
