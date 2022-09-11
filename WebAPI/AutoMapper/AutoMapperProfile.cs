using AutoMapper;
using Entities.Concrete;
using Entities.DTOs;

namespace WebAPI.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<RemoveCategoryDto, Category>();
        }
    }
}
