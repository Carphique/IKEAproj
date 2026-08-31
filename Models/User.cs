namespace ikea.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public List<Review> Reviews { get; set; } = new();

        public Cart? Cart { get; set; }
        public List<Wishlist> WishlistItems { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}