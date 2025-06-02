using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;

namespace FUNews.DAL.Repository
{
    public class SystemAccountRepository(FUNewsDbContext context) : BaseRepository<SystemAccount, short>(context), ISystemAccountRepository
    {
    }
}
