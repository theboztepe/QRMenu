using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
