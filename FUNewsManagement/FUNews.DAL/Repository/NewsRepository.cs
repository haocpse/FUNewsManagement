using FuNews.Modals.DTOs.Response;
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
    public class NewsRepository : BaseRepository<NewsArticle, String>, INewsRepository
    {
        private readonly FUNewsDbContext _context;

        public NewsRepository(FUNewsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<NewsArticle>> FindAllByDate(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<ReportItem>> GetReportByDateAsync(DateTime startDate, DateTime endDate, string groupBy)
        {
            var query = _context.NewsArticles
                .Where(n => n.CreatedDate.HasValue &&
                            n.CreatedDate.Value >= startDate &&
                            n.CreatedDate.Value <= endDate);

            var grouped = groupBy.ToLower() switch
            {
                "day" => query
                    .GroupBy(n => n.CreatedDate.Value.Date)
                    .AsEnumerable() // chuyển sang client
                    .Select(g => new ReportItem
                    {
                        Label = g.Key.ToString("dd/MM/yyyy"),
                        count = g.Count()
                    })
                    .OrderBy(r => r.Label)
                    .ToList(),

                "month" => query
                    .GroupBy(n => new { n.CreatedDate.Value.Year, n.CreatedDate.Value.Month })
                    .AsEnumerable()
                    .Select(g => new ReportItem
                    {
                        Label = $"{g.Key.Month:D2}/{g.Key.Year}",
                        count = g.Count()
                    })
                    .OrderBy(r => r.Label)
                    .ToList(),

                "year" => query
                    .GroupBy(n => n.CreatedDate.Value.Year)
                    .AsEnumerable()
                    .Select(g => new ReportItem
                    {
                        Label = g.Key.ToString(),
                        count = g.Count()
                    })
                    .OrderBy(r => r.Label)
                    .ToList(),

                _ => throw new ArgumentException("Invalid groupBy value")
            };

            return await Task.FromResult(grouped); // vì đã xử lý xong, không cần EF gọi tiếp DB
        }


    }
}
