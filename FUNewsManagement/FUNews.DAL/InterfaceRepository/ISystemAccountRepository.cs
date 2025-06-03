using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.DAL.Entity;

namespace FUNews.DAL.InterfaceRepository
{
    public interface ISystemAccountRepository : IBaseRepository<SystemAccount, short>
    {
        Task<SystemAccount> Login(String email, String password);
        Task<SystemAccount> GetByEmailAsync(string email);
    }
}
