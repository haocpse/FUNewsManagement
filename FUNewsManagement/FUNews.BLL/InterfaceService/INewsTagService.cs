using FuNews.Modals.DTOs.Request._Tag;
using FUNews.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.InterfaceService
{
    public interface INewsTagService
    {
        Task AddNewsTag(String newsId, List<int> tagIds);
        Task UpdateNewsTag(String newsId, List<NewsTagRequest> tagIds);

        Task<List<NewsTag>> GetAllByNewsIdAsync(String News);

    }
}
