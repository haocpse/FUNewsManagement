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
    }
}
