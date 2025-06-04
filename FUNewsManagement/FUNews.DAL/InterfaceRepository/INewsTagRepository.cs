using FUNews.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.InterfaceRepository
{
    public interface INewsTagRepository
    {
        Task AddAsync(NewsTag entity);
        Task DeleteAsync(string id);
        Task<List<NewsTag>> GetAllByNewsIdAsync(String News);
    }
}
