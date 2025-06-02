using AutoMapper;
using Azure.Core;
using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Response.News;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
    public class NewsService : BaseService<NewsArticle, String>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly INewsService _newsService;
        private readonly INewsTagService _newsTagService;
        private readonly INewsTagRepository _newsTagRepository;

        public NewsService(INewsRepository newsRepository, IMapper mapper, INewsService newsService, INewsTagService newsTagService, INewsTagRepository newsTagRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _newsService = newsService;
            _newsTagService = newsTagService;
            _newsTagRepository = newsTagRepository;
        }

        public async Task<NewsResponse> CreateNews(NewsRequest request)
        {
            NewsArticle news = new()
            {
                NewsArticleId = Guid.NewGuid().ToString(),
                NewsContent = request.NewsContent,
                NewsTitle = request.NewsTitle,  
                NewsSource = request.NewsSource,
                NewsStatus = false,
                CategoryId = request.CategoryId,
                CreatedDate = DateTime.Now,
                //CreatedById = ,
                ModifiedDate = DateTime.Now,
            };
            await _newsRepository.AddAsync(news);
            if (!request.TagIds.IsNullOrEmpty()) { 
                await _newsTagService.AddNewsTag(news.NewsArticleId, request.TagIds);
            }
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> UpdateNews(UpdateRequest request)
        {
            NewsArticle? news = await _newsRepository.GetByIdAsync(request.NewsArticleId);
            if (news != null) 
            { 
                if (!request.TagIds.IsNullOrEmpty())
                {
                    await _newsTagService.UpdateNewsTag(news.NewsArticleId, request.TagIds);
                }
                //news.UpdatedById = 
                news.ModifiedDate = DateTime.Now;
                await _newsRepository.UpdateAsync(news);
            }
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task DeleteNews(String id)
        {
            await _newsTagRepository.DeleteAsync(id);
            await _repository.DeleteAsync(id);
        }

        public async Task<List<NewsResponse>> GetOwnedNews(short id)
        {
            var newsList = await _newsRepository.GetOwnedNews(id);
            return _mapper.Map<List<NewsResponse>>(newsList);
        }

    }
}
