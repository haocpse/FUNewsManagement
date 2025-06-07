using AutoMapper;
using Azure.Core;
using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Request.Paging;
using FuNews.Modals.DTOs.Response.News;
using FuNews.Modals.DTOs.Response.Paging;
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
        private readonly ISystemAccountRepository _systemAccountRepository;

        public NewsService(INewsRepository newsRepository, IMapper mapper, INewsTagService newsTagService, INewsTagRepository newsTagRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository, ISystemAccountRepository systemAccountRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _newsTagService = newsTagService;
            _newsTagRepository = newsTagRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _systemAccountRepository = systemAccountRepository;
        }

        public async Task<NewsResponse> CreateNews(short id, NewsRequest request)
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
                CreatedById = id,
                ModifiedDate = DateTime.Now,
            };
            await _newsRepository.AddAsync(news);
            if (!request.TagIds.IsNullOrEmpty())
            {
                await _newsTagService.AddNewsTag(news.NewsArticleId, request.TagIds);
            }
            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> UpdateNews(short id, UpdateRequest request)
        {
            NewsArticle? news = await _newsRepository.GetByIdAsync(request.NewsArticleId);
            if (news != null)
            {
                if (!request.TagIds.IsNullOrEmpty())
                {
                    await _newsTagService.UpdateNewsTag(news.NewsArticleId, request.TagIds);
                }
                news.UpdatedById = id;
                news.ModifiedDate = DateTime.Now;
                news.CategoryId = request.CategoryId;
                news.NewsTitle = request.NewsTitle;
                news.NewsSource = request.NewsSource;
                news.Headline = request.Headline;
                news.NewsContent = request.NewsContent;
                await _newsRepository.UpdateAsync(news);
            }
            return await BuildNewsResponse(news);
        }

        public async Task DeleteNews(String id)
        {
            await _newsTagRepository.DeleteAsync(id);
            await _repository.DeleteAsync(id);
        }
        public async Task<NewsResponse> GetById(String id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            return await BuildNewsResponse(news);
        }

        public async Task<PageResult<NewsResponse>> GetOwnedNews(short id, PagingRequest request)
        {
            var (news, total) = await _newsRepository.GetOwnedNews(id, request.PageNumber, request.PageSize);
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                responses.Add(await BuildNewsResponse(item));
            }
            return new PageResult<NewsResponse>
            {
                Items = responses,
                TotalItems = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<PageResult<NewsResponse>> OverriedGetAllAsync(PagingRequest request)
        {
            var (news, total) = await _newsRepository.GetAllNewsForGuest(request.PageNumber, request.PageSize);
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                responses.Add(await BuildNewsResponse(item));
            }
            return new PageResult<NewsResponse>
            {
                Items = responses,
                TotalItems = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<NewsResponse> ApproveNewsAsync(String id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            news.NewsStatus = true;
            await _newsRepository.UpdateAsync(news);   
            return await BuildNewsResponse(news);
        }

        public async Task<PageResult<NewsResponse>> GetNewsPendingApproval(PagingRequest request)
        {
            var (news, total) = await _newsRepository.GetNewsPendingApproval(request.PageNumber, request.PageSize);
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                responses.Add(await BuildNewsResponse(item));
            }
            return new PageResult<NewsResponse>
            {
                Items = responses,
                TotalItems = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

        }

        public async Task<PageResult<NewsResponse>> GetAllForAdmin(PagingRequest request)
        {
            var (news, total) = await _newsRepository.GetAllNewsForAdmin(request.PageNumber, request.PageSize);
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                NewsResponse response = await BuildNewsResponse(item);
                responses.Add(response);
            }
            return new PageResult<NewsResponse>
            {
                Items = responses,
                TotalItems = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<PageResult<NewsResponse>> GetNewsByCategoryAsync(short? categoryId, PagingRequest request)
        {

            var (news, total) = await _newsRepository.GetNewsByCategoryAsync(request.PageNumber, request.PageSize, categoryId);
            List<NewsResponse> responses = new List<NewsResponse>();
            foreach (var item in news)
            {
                responses.Add(await BuildNewsResponse(item));
            }
            return new PageResult<NewsResponse>
            {
                Items = responses,
                TotalItems = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
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
                    IsActive = category.IsActive.Value,
                    ParentCategoryId = category.ParentCategoryId,
                },
                NewsStatus = item.NewsStatus,
                Tags = tagsRespone,
                CreatedDate = item.CreatedDate,
                AccountName = _systemAccountRepository.GetByIdAsync(item.CreatedById.Value).Result.AccountName
            };
        }

    }
}

