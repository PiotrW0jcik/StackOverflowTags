using Newtonsoft.Json;
using System.Net.Http;
using TagsService.Models;

namespace TagsService.ApiClient
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Tag>> GetTagsFromApiAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0");

            var urlTemplate = "https://api.stackexchange.com/2.3/tags?order=desc&sort=popular&site=stackoverflow&page={page}&pagesize={pagesize}";
            var tags = new List<Tag>();
            int page = 1;

            while (tags.Count < 1000)
            {
                var url = urlTemplate.Replace("{page}", page.ToString())
                                     .Replace("{pagesize}", "100");

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to fetch tags: {response.StatusCode}, Response: {responseBody}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<TagApiResponse>(json);
                var newTags = apiResponse.Items;

                tags.AddRange(newTags);

                if (apiResponse.HasMore)
                {
                    page++;
                }
                else
                {
                    break;
                }
            }

            return tags;
        }
    }
}
