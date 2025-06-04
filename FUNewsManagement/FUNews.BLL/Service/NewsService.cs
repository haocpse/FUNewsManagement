using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
    public class NewsService : BaseService<NewsArticle, String>, INewsRepository
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository) : base(newsRepository)
        {
            _newsRepository = newsRepository;
        }


    }
}
