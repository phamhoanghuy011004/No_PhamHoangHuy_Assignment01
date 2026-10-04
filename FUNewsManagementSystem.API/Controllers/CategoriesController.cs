using FUNewsManagementSystem.API.DTOs.Categories;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories.Select(MapToResponseDto));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);
            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(MapToResponseDto(category));
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? keyword, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.SearchAsync(keyword, cancellationToken);
            return Ok(categories.Select(MapToResponseDto));
        }

        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetActiveCategoriesAsync(cancellationToken);
            return Ok(categories.Select(MapToResponseDto));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                CategoryName = dto.CategoryName,
                CategoryDesciption = dto.CategoryDesciption,
                ParentCategoryID = dto.ParentCategoryID,
                IsActive = dto.IsActive ?? true
            };

            var (success, message) = await _categoryService.CreateAsync(category, cancellationToken);
            if (!success)
                return BadRequest(new { message });

            return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, MapToResponseDto(category));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(short id, [FromBody] CategoryUpdateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                CategoryID = id,
                CategoryName = dto.CategoryName,
                CategoryDesciption = dto.CategoryDesciption,
                ParentCategoryID = dto.ParentCategoryID,
                IsActive = dto.IsActive
            };

            var (success, message) = await _categoryService.UpdateAsync(category, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(short id, CancellationToken cancellationToken)
        {
            var (success, message) = await _categoryService.DeleteAsync(id, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                if (message.Contains("used by"))
                    return Conflict(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        private static CategoryResponseDto MapToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryID = category.ParentCategoryID,
                IsActive = category.IsActive
            };
        }
    }
}