using FUNewsManagementSystem.API.DTOs.News;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly INewsArticleService _newsService;

        public ReportsController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// GET /api/reports/statistic?startDate=2024-01-01&endDate=2024-12-31
        /// </summary>
        [HttpGet("statistic")]
        [ProducesResponseType(typeof(IEnumerable<NewsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStatistic(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken cancellationToken)
        {
            if (startDate > endDate)
                return BadRequest(new { message = "StartDate must be less than or equal to EndDate." });

            var news = await _newsService.GetNewsByDateRangeAsync(startDate, endDate, cancellationToken);

            var result = news.Select(n => new NewsResponseDto
            {
                NewsArticleID = n.NewsArticleID,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                CategoryID = n.CategoryID,
                CategoryName = n.Category?.CategoryName,
                NewsStatus = n.NewsStatus,
                CreatedByID = n.CreatedByID,
                CreatedByName = n.CreatedBy?.AccountName,
                ModifiedDate = n.ModifiedDate
            });

            return Ok(result);
        }
    }
}