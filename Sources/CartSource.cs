using Microsoft.EntityFrameworkCore;
using ikea.Data;
using ikea.DTO.Requests;
using ikea.DTO.Responses;
using ikea.Models;

namespace ikea.Sources
{
    public class CartSource
    {
        private readonly AppDbContext _context;

        public CartSource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartResponseDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p!.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return new CartResponseDto
            {
                CartId = cart.Id,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    ArticleNumber = i.Product?.ArticleNumber ?? string.Empty,
                    Price = i.Product?.Price ?? 0,
                    MainImageUrl = i.Product?.Images.FirstOrDefault()?.Url ?? string.Empty,
                    Quantity = i.Quantity
                }).ToList()
            };
        }

        public async Task AddToCartAsync(int userId, AddToCartDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int userId, int cartItemId, int quantity)
        {
            var item = await _context.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Id == cartItemId && i.Cart!.UserId == userId);

            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(int userId, int cartItemId)
        {
            var item = await _context.CartItems
                .Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Id == cartItemId && i.Cart!.UserId == userId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}