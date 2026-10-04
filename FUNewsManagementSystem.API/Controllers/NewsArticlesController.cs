using FUNewsManagementSystem.API.DTOs.News;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsService;

        public NewsArticlesController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Public: GET /api/newsarticles/active
        /// </summary>
        [HttpGet("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveNews(CancellationToken cancellationToken)
        {
            var news = await _newsService.GetActiveNewsAsync(cancellationToken);
            return Ok(news.Select(MapToResponseDto));
        }

        /// <summary>
        /// Staff: GET /api/newsarticles
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var news = await _newsService.GetAllAsync(cancellationToken);
            return Ok(news.Select(MapToResponseDto));
        }

        /// <summary>
        /// GET /api/newsarticles/{id}
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(NewsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var news = await _newsService.GetByIdAsync(id, cancellationToken);
            if (news == null)
                return NotFound(new { message = "News article not found." });

            // Public chỉ xem được Active
            if (news.NewsStatus != true && !User.IsInRole("Staff") && !User.IsInRole("Admin"))
                return NotFound(new { message = "News article not found." });

            return Ok(MapToResponseDto(news));
        }

        /// <summary>
        /// Staff: GET /api/newsarticles/search?keyword=xxx
        /// </summary>
        [HttpGet("search")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? keyword, CancellationToken cancellationToken)
        {
            var news = await _newsService.SearchAsync(keyword, cancellationToken);
            return Ok(news.Select(MapToResponseDto));
        }

        /// <summary>
        /// Staff: GET /api/newsarticles/my-history
        /// </summary>
        [HttpGet("my-history")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyHistory(CancellationToken cancellationToken)
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountIdClaim) || !short.TryParse(accountIdClaim, out short accountId))
                return Unauthorized(new { message = "Invalid token." });

            var news = await _newsService.GetNewsByCreatorAsync(accountId, cancellationToken);
            return Ok(news.Select(MapToResponseDto));
        }

        /// <summary>
        /// Staff: POST /api/newsarticles
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(typeof(NewsResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] NewsCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(accountIdClaim) || !short.TryParse(accountIdClaim, out short accountId))
                return Unauthorized(new { message = "Invalid token." });

            var news = new NewsArticle
            {
                NewsArticleID = dto.NewsArticleID,
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                NewsContent = dto.NewsContent,
                NewsSource = dto.NewsSource,
                CategoryID = dto.CategoryID,
                NewsStatus = dto.NewsStatus ?? true,
                CreatedByID = accountId,
                UpdatedByID = accountId
            };

            var (success, message) = await _newsService.CreateAsync(news, dto.TagIds, cancellationToken);

            if (!success)
            {
                if (message.Contains("already exists"))
                    return Conflict(new { message });
                return BadRequest(new { message });
            }

            var created = await _newsService.GetByIdAsync(dto.NewsArticleID, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = dto.NewsArticleID }, MapToResponseDto(created!));
        }

        /// <summary>
        /// Staff: PUT /api/newsarticles/{id}
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] NewsUpdateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            short? accountId = null;
            if (!string.IsNullOrEmpty(accountIdClaim) && short.TryParse(accountIdClaim, out short parsedId))
                accountId = parsedId;

            var news = new NewsArticle
            {
                NewsArticleID = id,
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                NewsContent = dto.NewsContent,
                NewsSource = dto.NewsSource,
                CategoryID = dto.CategoryID,
                NewsStatus = dto.NewsStatus,
                UpdatedByID = accountId
            };

            var (success, message) = await _newsService.UpdateAsync(news, dto.TagIds, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        /// <summary>
        /// Staff: DELETE /api/newsarticles/{id}
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var (success, message) = await _newsService.DeleteAsync(id, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        private static NewsResponseDto MapToResponseDto(NewsArticle news)
        {
            return new NewsResponseDto
            {
                NewsArticleID = news.NewsArticleID,
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                CreatedDate = news.CreatedDate,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryID = news.CategoryID,
                CategoryName = news.Category?.CategoryName,
                NewsStatus = news.NewsStatus,
                CreatedByID = news.CreatedByID,
                CreatedByName = news.CreatedBy?.AccountName,
                UpdatedByID = news.UpdatedByID,
                ModifiedDate = news.ModifiedDate,
                Tags = news.NewsTags?.Select(nt => new NewsTagDto
                {
                    TagID = nt.TagID,
                    TagName = nt.Tag?.TagName
                }).ToList() ?? new List<NewsTagDto>()
            };
        }
    }
}