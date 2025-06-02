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
    public class NewsTagRepository : INewsTagRepository
    {
        protected readonly FUNewsDbContext _context;
        protected readonly DbSet<NewsTag> _dbSet;

        public NewsTagRepository(FUNewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<NewsTag>();
        }

        public async Task AddAsync(NewsTag entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var tagsToRemove = await _context.NewsTags
                .Where(nt => nt.NewsArticleId == id)
                .ToListAsync();

            _context.NewsTags.RemoveRange(tagsToRemove);

            await _context.SaveChangesAsync();
        }
    }
}
