using FUNewsManagementSystem.API.DTOs.Tags;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TagResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var tags = await _tagService.GetAllAsync(cancellationToken);
            var result = tags.Select(t => new TagResponseDto
            {
                TagID = t.TagID,
                TagName = t.TagName,
                Note = t.Note
            });
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TagResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? keyword, CancellationToken cancellationToken)
        {
            var tags = await _tagService.SearchAsync(keyword, cancellationToken);
            var result = tags.Select(t => new TagResponseDto
            {
                TagID = t.TagID,
                TagName = t.TagName,
                Note = t.Note
            });
            return Ok(result);
        }
    }
}