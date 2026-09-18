using ikea.Data;
using ikea.DTO.Requests;
using ikea.DTO.Responses;
using ikea.Models;
using Microsoft.EntityFrameworkCore;

namespace ikea.Sources
{
    public class ProductSource
    {
        private readonly AppDbContext _context;

        public ProductSource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(int? categoryId, string? search)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || p.ArticleNumber.Contains(search));

            return await query.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                ArticleNumber = p.ArticleNumber,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Dimensions = p.Dimensions,
                Color = p.Color,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : string.Empty,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count,
                Images = p.Images.Select(i => i.Url).ToList()
            }).ToListAsync();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null) return null;

            return new ProductResponseDto
            {
                Id = p.Id,
                ArticleNumber = p.ArticleNumber,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Dimensions = p.Dimensions,
                Color = p.Color,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count,
                Images = p.Images.Select(i => i.Url).ToList()
            };
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                ArticleNumber = dto.ArticleNumber,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Dimensions = dto.Dimensions,
                Color = dto.Color,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                Images = dto.ImageUrls.Select(url => new ProductImage { Url = url }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(product.Id))!;
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return (false, "Товар не знайдено");

            var relatedCartItems = await _context.CartItems.Where(ci => ci.ProductId == id).ToListAsync();
            _context.CartItems.RemoveRange(relatedCartItems);
            _context.Reviews.RemoveRange(product.Reviews);
            _context.Images.RemoveRange(product.Images);
            _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateException)
            {
                return (false, "Неможливо видалити товар: він уже є в оформленому замовленні одного з покупців.");
            }
        }
    }
}