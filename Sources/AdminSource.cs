using ikea.Data;
using ikea.DTO.Requests;
using ikea.DTO.Responses;
using ikea.Models;
using Microsoft.EntityFrameworkCore;

namespace ikea.Sources
{
	public class AdminSource
	{
		private readonly AppDbContext _context;

		public AdminSource(AppDbContext context)
		{
			_context = context;
		}

		// --- Управління товарами ---
		public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
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

			return (await GetProductByIdAsync(product.Id))!;
		}

		public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto dto)
		{
			var product = await _context.Products
				.Include(p => p.Images)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null) return null;

			product.ArticleNumber = dto.ArticleNumber;
			product.Name = dto.Name;
			product.Description = dto.Description;
			product.Price = dto.Price;
			product.Dimensions = dto.Dimensions;
			product.Color = dto.Color;
			product.StockQuantity = dto.StockQuantity;
			product.CategoryId = dto.CategoryId;

			_context.Images.RemoveRange(product.Images);
			product.Images = dto.ImageUrls.Select(url => new ProductImage { Url = url }).ToList();

			await _context.SaveChangesAsync();

			return await GetProductByIdAsync(product.Id);
		}

		private async Task<ProductResponseDto?> GetProductByIdAsync(int id)
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

		// --- Управління замовленнями ---
		public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
		{
			var orders = await _context.Orders
				.Include(o => o.Items)
				.ThenInclude(i => i.Product)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();

			return orders.Select(o => new OrderResponseDto
			{
				OrderId = o.Id,
				OrderDate = o.OrderDate,
				TotalAmount = o.TotalAmount,
				Status = o.Status,
				DeliveryAddress = o.DeliveryAddress,
				Items = o.Items.Select(i => new OrderItemResponseDto
				{
					ProductId = i.ProductId,
					ProductName = i.Product?.Name ?? string.Empty,
					UnitPrice = i.UnitPrice,
					Quantity = i.Quantity
				}).ToList()
			});
		}

		public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
		{
			var order = await _context.Orders.FindAsync(orderId);
			if (order == null) return false;

			order.Status = status;
			await _context.SaveChangesAsync();
			return true;
		}

		// --- Аналітика / Статистика ---
		public async Task<object> GetDashboardStatsAsync()
		{
			var totalUsers = await _context.Users.CountAsync();
			var totalProducts = await _context.Products.CountAsync();
			var totalOrders = await _context.Orders.CountAsync();
			var totalRevenue = await _context.Orders
				.Where(o => o.Status != "Cancelled")
				.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

			return new
			{
				TotalUsers = totalUsers,
				TotalProducts = totalProducts,
				TotalOrders = totalOrders,
				TotalRevenue = totalRevenue
			};
		}
	}
}