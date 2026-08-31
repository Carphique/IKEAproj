namespace ikea.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ArticleNumber { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public decimal Price { get; set; }
        public string Dimensions { get; set; } = string.Empty; 
        public string Color { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public List<Review> Reviews { get; set; } = new();

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<ProductImage> Images { get; set; } = new();
    }
}