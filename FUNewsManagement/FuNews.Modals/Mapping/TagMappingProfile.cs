using AutoMapper;
using FUNews.DAL.Entity;
using FUNews.Modals.DTOs.Response;
using FUNewsManagement.Models.Request;

namespace FUNews.Modals.Mapping
{
    public class TagMappingProfile : Profile
    {
        public TagMappingProfile()
        {
            CreateMap<Tag, TagResponse>();
            CreateMap<TagRequest, Tag>();
        }
    }
}