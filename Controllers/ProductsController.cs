using ikea.DTO.Requests;
using ikea.Sources;
using Microsoft.AspNetCore.Mvc;

namespace ikea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductSource _productSource;

        public ProductsController(ProductSource productSource)
        {
            _productSource = productSource;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId, [FromQuery] string? search)
        {
            var products = await _productSource.GetAllAsync(categoryId, search);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productSource.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Товар не знайдено" });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var createdProduct = await _productSource.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productSource.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}