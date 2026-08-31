namespace ikea.DTO.Responses
{
    public class CartItemResponseDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ArticleNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    public class CartResponseDto
    {
        public int CartId { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    }
}