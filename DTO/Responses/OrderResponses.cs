namespace ikea.DTO.Responses
{
    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}   