using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FUNews.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.BLL.Service
{
    public class CategoryService : BaseService<Category, short>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepositoy;

        public CategoryService(ICategoryRepository categoryRepositoy) : base(categoryRepositoy)
        { 
            _categoryRepositoy = categoryRepositoy;
        }



    }
}
