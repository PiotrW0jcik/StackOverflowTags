using Microsoft.AspNetCore.Mvc;
using TagsService.Models;
using TagsService.Services;

namespace TagsService.Controllers
{
    /// <summary>
    /// Handles operations related to tags.
    /// </summary>
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Retrieves a paginated list of tags from the cache or by refreshing from StackOverflow.
        /// </summary>
        /// <param name="page">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of tags per page (default is 10).</param>
        /// <param name="sortBy">The field to sort by (either 'name' or 'percentage').</param>
        /// <param name="descending">Whether to sort in descending order.</param>
        /// <returns>A paginated list of tags.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Tag>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTags(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "name",
            [FromQuery] bool descending = false)
        {
            var tags = await _tagService.GetTagsAsync(page, pageSize, sortBy, descending);
            return Ok(tags);
        }

        /// <summary>
        /// Refreshes the tags data by fetching new data from the StackOverflow API and caching it.
        /// </summary>
        /// <returns>Returns a confirmation message indicating the refresh status.</returns>
        [HttpPost("refresh")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshTags()
        {
            await _tagService.RefreshTagsAsync();
            return Ok("Tags refreshed");
        }
    }
}
