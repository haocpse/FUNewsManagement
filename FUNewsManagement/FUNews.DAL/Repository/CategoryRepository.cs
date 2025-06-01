using FUNews.DAL.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNews.DAL.Entity;

namespace FUNews.DAL.Repository
{
    public class CategoryRepository(FUNewsDbContext context) : BaseRepository<Category,short>(context), ICategoryRepository
    {
        
    }
}
