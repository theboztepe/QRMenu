using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.Category;
using Entities.DTOs.Product;

namespace WebAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<RemoveCategoryDto, Category>();

            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<RemoveProductDto, Product>();
        }
    }
}
