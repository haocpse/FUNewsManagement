using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Repository
{
    public class NewsRepository(FUNewsDbContext context) : BaseRepository<NewsArticle, String>(context), INewsRepository
    {


        public Task<List<NewsArticle>> GetOwnedNews(short id) 
        {
            return _context.NewsArticles
                  .Where(n => n.CreatedById == id)
                  .ToListAsync();
        }   

    }
}
