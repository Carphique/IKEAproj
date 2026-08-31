using Microsoft.EntityFrameworkCore;
using ikea.Data;
using ikea.DTO.Requests;
using ikea.DTO.Responses;
using ikea.Models;

namespace ikea.Sources
{
    public class CategorySource
    {
        private readonly AppDbContext _context;

        public CategorySource(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();

            return categories.Select(MapToDto);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                ParentCategoryId = dto.ParentCategoryId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return MapToDto(category);
        }

        private static CategoryResponseDto MapToDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl,
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories.Select(MapToDto).ToList()
            };
        }
    }
}