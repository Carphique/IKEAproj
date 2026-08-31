using System.ComponentModel.DataAnnotations;

namespace ikea.DTO.Requests
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemQuantityDto
    {
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}