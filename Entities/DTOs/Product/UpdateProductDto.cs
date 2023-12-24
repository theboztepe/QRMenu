﻿using Entities.Concrete;

namespace Entities.DTOs.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public CurrencyType Currency { get; set; }
        public string? Image { get; set; }
    }
}
