using System.ComponentModel.DataAnnotations;
namespace ikea.DTO.Requests
{
    public class UpdateProductDto : CreateProductDto
    {
        [Required]
        public int Id { get; set; }
    }
    public class UpdateOrderStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty; // e.g. "Processing", "Shipped", "Delivered", "Cancelled"
    }
}