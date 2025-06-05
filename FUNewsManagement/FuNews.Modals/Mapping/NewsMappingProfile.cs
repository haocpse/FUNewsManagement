using AutoMapper;
using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Response.News;
using FUNews.DAL.Entity;
using FUNews.Modals.DTOs.Response;
using FUNewsManagement.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.Mapping
{
    public class NewsMappingProfile : Profile
    {
        public NewsMappingProfile()
        {
            CreateMap<NewsArticle, NewsResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.NewsTags.Select(nt => nt.Tag)));
            CreateMap<NewsRequest, NewsArticle>();
        }

    }
}
