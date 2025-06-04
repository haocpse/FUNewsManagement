using AutoMapper;
using FUNews.DAL.Entity;
using FUNews.Modals.DTOs.Request;
using FUNews.Modals.DTOs.Response;
using FUNewsManagement.Models.Request;

namespace FUNews.Modals.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category,CategoryResponse>();
            CreateMap<CategoryRequest,Category>();
        }
    }
}