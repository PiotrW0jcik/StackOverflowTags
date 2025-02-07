using TagsService.ApiClient;
using TagsService.Models;

namespace TagsService.Services
{
    public class TagService : ITagService
    {
        private readonly IRedisCache _cache;
        private readonly IApiClient _apiClient;
        private readonly ILogger<TagService> _logger;

        private const int MaxTagsCount = 1000;

        public TagService(IRedisCache cache, IApiClient apiClient, ILogger<TagService> logger)
        {
            _cache = cache;
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(int page, int pageSize, string sortBy, bool descending)
        {
            var tags = await _cache.GetTagsAsync();
            if (tags == null || !tags.Any())
            {
                await RefreshTagsAsync();
                tags = await _cache.GetTagsAsync();
            }

            // Sorting logic
            tags = sortBy switch
            {
                "name" => descending ? tags.OrderByDescending(t => t.Name) : tags.OrderBy(t => t.Name),
                "percentage" => descending ? tags.OrderByDescending(t => t.Percentage) : tags.OrderBy(t => t.Percentage),
                _ => tags.OrderBy(t => t.Name),
            };

            return tags.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task RefreshTagsAsync()
        {
            var tags = await _apiClient.GetTagsFromApiAsync();

            // Calculate percentage for each tag
            var totalCount = tags.Sum(tag => tag.Count);
            foreach (var tag in tags)
            {
                tag.Percentage = Math.Round((double)tag.Count / totalCount * 100, 2);
            }

            await _cache.SetTagsAsync(tags);
            _logger.LogInformation("Successfully refreshed and cached tags.");
        }
    }
}
