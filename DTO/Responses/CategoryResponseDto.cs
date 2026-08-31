namespace ikea.DTO.Responses
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public List<CategoryResponseDto> SubCategories { get; set; } = new();
    }
}