using Moq;
using TagsService.Services;
using TagsService.Models;
using TagsService.ApiClient;
using Microsoft.Extensions.Logging;

namespace TagsServiceTests
{
    [TestFixture]
    public class TagServiceTests
    {
        private Mock<IRedisCache> _mockCache;
        private Mock<IApiClient> _mockApiClient;
        private Mock<ILogger<TagService>> _mockLogger;
        private TagService _tagService;

        [SetUp]
        public void Setup()
        {
            _mockCache = new Mock<IRedisCache>();
            _mockApiClient = new Mock<IApiClient>();
            _mockLogger = new Mock<ILogger<TagService>>();

            _tagService = new TagService(
                _mockCache.Object,
                _mockApiClient.Object,
                _mockLogger.Object
            );
        }

        [Test]
        public async Task GetTagsAsync_ShouldReturnTagsFromCache_WhenTagsArePresent()
        {
            var cachedTags = new List<Tag>
            {
                new Tag { Name = "C#", Count = 1000, Percentage = 50 },
                new Tag { Name = "Java", Count = 1000, Percentage = 50 }
            };

            _mockCache.Setup(c => c.GetTagsAsync()).ReturnsAsync(cachedTags);

            var result = await _tagService.GetTagsAsync(1, 10, "name", false);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // U¿ywamy Count() zamiast Count
        }

        [Test]
        public async Task RefreshTagsAsync_ShouldFetchAndStoreTags_WhenCalled()
        {
            var fetchedTags = new List<Tag>
            {
                new Tag { Name = "C#", Count = 1000 },
                new Tag { Name = "Java", Count = 1000 }
            };

            _mockCache.Setup(c => c.SetTagsAsync(It.IsAny<IEnumerable<Tag>>())).Returns(Task.CompletedTask);
            _mockApiClient.Setup(api => api.GetTagsFromApiAsync()).ReturnsAsync(fetchedTags);

            await _tagService.RefreshTagsAsync();

            _mockCache.Verify(c => c.SetTagsAsync(It.IsAny<IEnumerable<Tag>>()), Times.Once);
        }

        [Test]
        public async Task GetTagsAsync_ShouldCallApi_WhenCacheIsEmpty()
        {
            _mockCache.Setup(c => c.GetTagsAsync()).ReturnsAsync(new List<Tag>());
            var fetchedTags = new List<Tag>
            {
                new Tag { Name = "C#", Count = 1000 },
                new Tag { Name = "Java", Count = 1000 }
            };
            _mockApiClient.Setup(api => api.GetTagsFromApiAsync()).ReturnsAsync(fetchedTags);

            var result = await _tagService.GetTagsAsync(1, 10, "name", false);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // U¿ywamy Count() zamiast Count
            _mockCache.Verify(c => c.GetTagsAsync(), Times.Once);
            _mockApiClient.Verify(api => api.GetTagsFromApiAsync(), Times.Once);
            _mockCache.Verify(c => c.SetTagsAsync(It.IsAny<IEnumerable<Tag>>()), Times.Once);
        }
    }
}
