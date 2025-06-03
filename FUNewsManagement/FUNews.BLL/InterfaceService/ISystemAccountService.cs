using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.DAL.Entity;

namespace FUNews.BLL.InterfaceService
{
    public interface ISystemAccountService : IBaseService<SystemAccount, short>
    {
        Task<AccountDetailResponse> login(LoginRequest request);
        Task<AccountDetailResponse> UpdateAccount(UpdateAccountRequest request, bool isAdmin);
        Task<AccountDetailResponse> GetAccountById(short id);
        Task<AccountListResponse> GetAllAccounts();
    }
}
