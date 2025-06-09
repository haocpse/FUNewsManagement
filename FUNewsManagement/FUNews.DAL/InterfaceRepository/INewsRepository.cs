using FuNews.Modals.DTOs.Response;
using FuNews.Modals.DTOs.Response.News;
using FUNews.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.InterfaceRepository
{
    public interface INewsRepository : IBaseRepository<NewsArticle, String>
    {

        Task<List<NewsArticle>> FindAllByDate(DateTime startDate, DateTime endDate);
        Task<List<ReportItem>> GetReportByDateAsync(DateTime startDate, DateTime endDate, string groupBy);
        Task<(List<NewsArticle> Items, int TotalCount)> GetOwnedNews(short id, bool? status, int pageNumber, int pageSize);
        Task<(List<NewsArticle> Items, int TotalCount)> GetAllNewsForGuest(int pageNumber, int pageSize);

        Task<(List<NewsArticle> Items, int TotalCount)> GetAllNewsForAdmin(bool? status, int pageNumber, int pageSize);
        Task<(List<NewsArticle> Items, int TotalCount)> GetNewsPendingApproval(int pageNumber, int pageSize);
        Task<NewsArticle> GetNewsByCategoryId(short categoryId);

        Task<(List<NewsArticle> Items, int TotalCount)> GetNewsByCategoryAsync(int pageNumber, int pageSize, short? categoryId);

    }
}
