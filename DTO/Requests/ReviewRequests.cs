using System.ComponentModel.DataAnnotations;

namespace ikea.DTO.Requests
{
    public class CreateReviewDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;
    }
}