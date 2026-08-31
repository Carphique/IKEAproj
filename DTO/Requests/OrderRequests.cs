using System.ComponentModel.DataAnnotations;

namespace ikea.DTO.Requests
{
    public class CreateOrderDto
    {
        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;
    }
}