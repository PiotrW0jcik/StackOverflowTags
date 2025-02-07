using NUnit.Framework;
using TagsService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TagsService.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TagsServiceTests.Integration
{
    [TestFixture]
    public class TagServiceIntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetTags_ShouldReturnTags_WhenCalled()
        {
            var response = await _client.GetAsync("/api/tags?page=1&pageSize=10");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<List<Tag>>(content);

            Assert.IsNotNull(tags);
            Assert.IsTrue(tags.Count > 0);
        }

        [Test]
        public async Task RefreshTags_ShouldFetchAndCacheTags_WhenCalled()
        {
            var response = await _client.PostAsync("/api/tags/refresh", null);

            response.EnsureSuccessStatusCode();

            var redis = _factory.Services.GetRequiredService<IConnectionMultiplexer>();
            var db = redis.GetDatabase();
            var tagsJson = await db.StringGetAsync("tags");
            Assert.IsFalse(tagsJson.IsNullOrEmpty);
            var tags = JsonConvert.DeserializeObject<List<Tag>>(tagsJson);
            Assert.IsTrue(tags.Count > 0);
        }
    }
}
