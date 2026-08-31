namespace ikea.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }

    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string DeliveryAddress { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}