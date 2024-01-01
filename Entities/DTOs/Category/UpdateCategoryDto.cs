namespace Entities.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public int TopCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
    }
}
