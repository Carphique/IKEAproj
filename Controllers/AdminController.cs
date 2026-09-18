using ikea.DTO.Requests;
using ikea.Sources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ikea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminSource _adminSource;
        private readonly ProductSource _productSource;
        private readonly CategorySource _categorySource;

        public AdminController(
            AdminSource adminSource,
            ProductSource productSource,
            CategorySource categorySource)
        {
            _adminSource = adminSource;
            _productSource = productSource;
            _categorySource = categorySource;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminSource.GetDashboardStatsAsync();
            return Ok(stats);
        }

        // Створити товар
        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var created = await _adminSource.CreateProductAsync(dto);
            return Ok(created);
        }

        [HttpPut("products/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _adminSource.UpdateProductAsync(id, dto);
            if (updated == null) return NotFound(new { message = "Товар не знайдено" });

            return Ok(updated);
        }

        [HttpDelete("products/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var (success, error) = await _productSource.DeleteAsync(id);
            if (!success)
                return BadRequest(new { message = error });

            return NoContent();
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _adminSource.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPatch("orders/{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            var success = await _adminSource.UpdateOrderStatusAsync(id, dto.Status);
            if (!success) return NotFound(new { message = "Замовлення не знайдено" });

            return Ok(new { message = "Статус замовлення оновлено" });
        }
    }
}