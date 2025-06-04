using FuNews.Modals.DTOs.Response.News;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.Modals.DTOs.Response;
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

        private readonly ICategoryRepository _categoryRepository;

        private readonly INewsTagRepository _newsTagRepository;

        private readonly ITagRepository _tagRepository;

        public NewsService(INewsRepository newsRepository, ICategoryRepository categoryRepository, INewsTagRepository newsTagRepository, ITagRepository tagRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _newsTagRepository = newsTagRepository;
            _tagRepository = tagRepository;
        }

        public async Task<List<NewsResponse>> GetAllNews()
        {
            var news = await _newsRepository.GetAllAsync();
            List<NewsResponse> newsResponse = new List<NewsResponse>();

            foreach (var item in news)
            {
                var category = await _categoryRepository.GetByIdAsync(item.CategoryId.Value);
                var tags = await _newsTagRepository.GetAllByNewsIdAsync(item.NewsArticleId);
                List<TagResponse> tagResponses =  new List<TagResponse>();
                foreach (var tag in tags)
                {
                    var tag1 = await _tagRepository.GetByIdAsync(tag.TagId);
                    TagResponse tagResponse = new()
                    {
                        TagId = tag.TagId,
                        TagName = tag1.TagName,
                        Note = tag1.Note,
                    };
                    tagResponses.Add(tagResponse);

                }
                NewsResponse response = new()
                {
                    NewsTitle = item.NewsTitle,
                    Headline = item.Headline,
                    NewsContent = item.NewsContent,
                    NewsSource = item.NewsSource,
                    NewsStatus = item.NewsStatus,
                    CreatedDate = item.CreatedDate,
                    Category = new()
                    {
                        CategoryId = item.CategoryId.Value,
                        CategoryName = category.CategoryName,
                        CategoryDescription = category.CategoryDescription,
                        ParentCategoryId = category.ParentCategoryId.Value,
                        IsActive = category.IsActive,
                    },
                    Tags = tagResponses

                };
                newsResponse.Add(response);
            }
            return newsResponse;
        }
    }
}
