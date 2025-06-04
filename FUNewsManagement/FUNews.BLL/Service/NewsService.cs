using AutoMapper;
using Azure.Core;
using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Response.News;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.Modals.DTOs.Response;
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
        private readonly INewsTagService _newsTagService;
        private readonly INewsTagRepository _newsTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public NewsService(INewsRepository newsRepository, IMapper mapper, INewsTagService newsTagService, INewsTagRepository newsTagRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _newsTagService = newsTagService;
            _newsTagRepository = newsTagRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task<NewsResponse> CreateNews(NewsRequest request)
        {
            NewsArticle news = new()
            {
                NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 20),
                NewsContent = request.NewsContent,
                Headline = request.Headline,
                NewsTitle = request.NewsTitle,
                NewsSource = request.NewsSource,
                NewsStatus = false,
                CategoryId = request.CategoryId,
                CreatedDate = DateTime.Now,
                //CreatedById = ,
                ModifiedDate = DateTime.Now,
            };
            await _newsRepository.AddAsync(news);
            if (!request.TagIds.IsNullOrEmpty())
            {
                await _newsTagService.AddNewsTag(news.NewsArticleId, request.TagIds);
            }
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> UpdateNews(UpdateRequest request)
        {
            NewsArticle? news = await _newsRepository.GetByIdAsync(request.NewsArticleId);
            if (news != null)
            {
                if (!request.Tags.IsNullOrEmpty())
                {
                    await _newsTagService.UpdateNewsTag(news.NewsArticleId, request.Tags);
                }
                //news.UpdatedById = 
                news.ModifiedDate = DateTime.Now;
                await _newsRepository.UpdateAsync(news);
            }
            return await BuildNewsResponse(news);
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

        public async Task<NewsResponse> GetById(String id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            return await BuildNewsResponse(news);
        }

        public async Task<List<NewsResponse>> OverriedGetAllAsync()
        {
            var news = await _newsRepository.GetAllAsync();
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                NewsResponse response = await BuildNewsResponse(item);
                responses.Add( response );
            }
            return responses;

        }

        private async Task<NewsResponse> BuildNewsResponse(NewsArticle item)
        {
            Category? category = await _categoryRepository.GetByIdAsync(item.CategoryId.Value);
            List<NewsTag> tags = await _newsTagRepository.GetAllByNewsIdAsync(item.NewsArticleId);
            List<TagResponse> tagsRespone = new List<TagResponse>();
            foreach (NewsTag tag in tags)
            {
                Tag? currentTag = await _tagRepository.GetByIdAsync(tag.TagId);
                tagsRespone.Add(
                    new()
                    {
                        TagId = tag.TagId,
                        TagName = currentTag.TagName,
                        Note = currentTag.Note,
                    }
                    );
            }
            return new()
            {
                NewsArticleId = item.NewsArticleId,
                NewsTitle = item.NewsTitle,
                Headline = item.Headline,
                NewsContent = item.NewsContent,
                NewsSource = item.NewsSource,
                Category = new()
                {
                    CategoryId = category.CategoryId,
                    CategoryDescription = category.CategoryDescription,
                    CategoryName = category.CategoryName,
                    IsActive = category.IsActive,
                    ParentCategoryId = category.ParentCategoryId,
                },
                NewsStatus = item.NewsStatus,
                Tags = tagsRespone,
                CreatedDate = item.CreatedDate
            };
        }
    }
}
