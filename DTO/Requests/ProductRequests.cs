using System.ComponentModel.DataAnnotations;

namespace ikea.DTO.Requests
{
    public class CreateProductDto
    {
        [Required]
        public string ArticleNumber { get; set; } = string.Empty; 

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        public string Dimensions { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }

        public List<string> ImageUrls { get; set; } = new();
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
    }
}