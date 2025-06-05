using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Response.News;
using FUNews.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.InterfaceService
{
    public interface INewsService : IBaseService<NewsArticle, String>
    {
        Task<NewsResponse> CreateNews(NewsRequest request);
        Task<NewsResponse> UpdateNews(UpdateRequest request);
        Task DeleteNews(String id);
        Task<List<NewsResponse>> GetOwnedNews(short id);

        Task<NewsResponse> GetById(String id);

        Task<List<NewsResponse>> OverriedGetAllAsync();

        Task<NewsResponse> ApproveNewsAsync(String id);
        Task<List<NewsResponse>> GetNewsPendingApproval();
    }
}
