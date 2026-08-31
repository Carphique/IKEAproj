namespace ikea.DTO.Responses
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}