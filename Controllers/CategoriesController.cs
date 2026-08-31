using Microsoft.AspNetCore.Mvc;
using ikea.DTO.Requests;
using ikea.Sources;

namespace ikea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategorySource _categorySource;

        public CategoriesController(CategorySource categorySource)
        {
            _categorySource = categorySource;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categorySource.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var category = await _categorySource.CreateAsync(dto);
            return Ok(category);
        }
    }
}