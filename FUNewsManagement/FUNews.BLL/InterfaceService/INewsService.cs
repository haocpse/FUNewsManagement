using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Request.Paging;
using FuNews.Modals.DTOs.Response.News;
using FuNews.Modals.DTOs.Response.Paging;
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
        Task<NewsResponse> CreateNews(short id, NewsRequest request);
        Task<NewsResponse> UpdateNews(short id, UpdateRequest request);
        Task DeleteNews(String id);
        Task<List<NewsResponse>> GetOwnedNews(short id);
        Task<NewsResponse> GetById(String id);

        Task<PageResult<NewsResponse>> OverriedGetAllAsync(PagingRequest request);
        Task<List<NewsResponse>> GetAllForAdmin();

        Task<NewsResponse> ApproveNewsAsync(String id);
        Task<List<NewsResponse>> GetNewsPendingApproval();

        Task<PageResult<NewsResponse>> GetNewsByCategoryAsync(short? categoryId, PagingRequest request);

    }
}
