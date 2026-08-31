namespace ikea.DTO.Responses
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ArticleNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public List<string> Images { get; set; } = new();
    }
}