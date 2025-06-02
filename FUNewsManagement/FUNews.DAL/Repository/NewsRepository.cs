using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Repository
{
    public class NewsRepository(FUNewsDbContext context) : BaseRepository<NewsArticle, String>(context), INewsRepository
    {

    }
}
