using FuNews.Modals.DTOs.Response;
using FuNews.Modals.DTOs.Response.News;
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
				.Where(a => a.CreatedDate.HasValue && a.CreatedDate >= startDate && a.CreatedDate <= endDate);

			var grouped = groupBy.ToLower() switch
			{
				"day" => await query
					.GroupBy(a => a.CreatedDate!.Value.Date)
					.Select(g => new ReportItem
					{
						Label = g.Key.ToString("dd/MM/yyyy"),
						count = g.Count()
					})
					.OrderBy(r => r.Label)
					.ToListAsync(),

				"month" => await query
					.GroupBy(a => new { a.CreatedDate!.Value.Year, a.CreatedDate!.Value.Month })
					.Select(g => new ReportItem
					{
						Label = $"Tháng {g.Key.Month}/{g.Key.Year}",
						count = g.Count()
					})
					.OrderBy(r => r.Label)
					.ToListAsync(),

				"year" => await query
					.GroupBy(a => a.CreatedDate!.Value.Year)
					.Select(g => new ReportItem
					{
						Label = g.Key.ToString(),
						count = g.Count()
					})
					.OrderBy(r => r.Label)
					.ToListAsync(),

				_ => throw new ArgumentException("Invalid groupBy value")
			};
			return grouped;
		}
        public Task<List<NewsArticle>> GetOwnedNews(short id)
        {
            return _context.NewsArticles
                  .Where(n => n.CreatedById == id)
                  .OrderByDescending(n => n.CreatedDate)
                  .ToListAsync();
        }

		public async Task<List<NewsArticle>> GetAllNewsForGuest()
		{
			return await _context.NewsArticles
				.Where(n => n.NewsStatus.Value == true)
				.OrderByDescending(n => n.CreatedDate)
				.ToListAsync();
		}

        public async Task<List<NewsArticle>> GetNewsPendingApproval()
        {
            return await _context.NewsArticles
                .Where(n => n.NewsStatus.Value == false)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsByCategoryId(short id)
        {
            return await _context.NewsArticles
                .Where(n => n.CategoryId == id)
                .OrderByDescending(n => n.CreatedDate)
                .FirstOrDefaultAsync();
        }
    }
}
