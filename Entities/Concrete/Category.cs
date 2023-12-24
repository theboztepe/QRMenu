using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class Category : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int TopCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
