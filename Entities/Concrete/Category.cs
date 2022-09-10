using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class Category : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int TopCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
