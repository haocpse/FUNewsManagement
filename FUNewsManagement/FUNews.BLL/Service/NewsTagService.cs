using FUNews.BLL.InterfaceService;
using FUNews.DAL.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
    public class NewsTagService : INewsTagService
    {
        private readonly INewsTagRepository _newsTagRepository;

        public NewsTagService(INewsTagRepository newsTagRepository)
        {
            _newsTagRepository = newsTagRepository;
        }

        public async Task AddNewsTag(String newsId, List<int> tagIds)
        {
            foreach (var tagId in tagIds) 
            {
                await _newsTagRepository.AddAsync(
                    new()
                    {
                        NewsArticleId = newsId,
                        TagId = tagId,
                    });
            }
        }

        public async Task UpdateNewsTag(String newsId, List<int> tagIds)
        {
            await _newsTagRepository.DeleteAsync(newsId);
            foreach (var tagId in tagIds)
            {
                await _newsTagRepository.AddAsync(
                   new()
                   {
                       NewsArticleId = newsId,
                       TagId = tagId,
                   });
            }
        }
    }
}
