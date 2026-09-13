using ECommerceStore.Dtos.CategoryDtos;
using ECommerceStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpGet("/categoryById")]
        public async Task<IActionResult> GetCategoryById(int categoryId, [FromServices] ICategoryService categoryService)
        {
            try
            {
                var category = await categoryService.GetCategoryByIdAsync(categoryId);
                return Ok(category);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("/categoriesNames")]
        public async Task<ActionResult<List<GetCategoriesDto>>> GetCategories()
        {
            var categories = await _service.GetCategoriesNames();
            return Ok(categories);
        }
    }
}
