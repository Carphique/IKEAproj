using ikea.DTO.Requests;
using ikea.Sources;
using Microsoft.AspNetCore.Mvc;

namespace ikea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartSource _cartSource;

        public CartController(CartSource cartSource)
        {
            _cartSource = cartSource;
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartSource.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId:int}/items")]
        public async Task<IActionResult> AddItem(int userId, [FromBody] AddToCartDto dto)
        {
            await _cartSource.AddToCartAsync(userId, dto);
            return Ok(new { message = "Товар додано у кошик" });
        }

        [HttpPut("{userId:int}/items/{cartItemId:int}")]
        public async Task<IActionResult> UpdateQuantity(int userId, int cartItemId, [FromBody] UpdateCartItemQuantityDto dto)
        {
            await _cartSource.UpdateQuantityAsync(userId, cartItemId, dto.Quantity);
            return NoContent();
        }

        [HttpDelete("{userId:int}/items/{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(int userId, int cartItemId)
        {
            await _cartSource.RemoveFromCartAsync(userId, cartItemId);
            return NoContent();
        }
    }
}