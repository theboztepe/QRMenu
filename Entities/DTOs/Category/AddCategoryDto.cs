namespace Entities.DTOs.Category
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TopCategoryId { get; set; }
        public string? Image { get; set; }
    }
}
