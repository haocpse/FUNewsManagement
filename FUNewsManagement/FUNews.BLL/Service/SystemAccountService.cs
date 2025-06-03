using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;

namespace FUNews.BLL.Service
{
    public class SystemAccountService(ISystemAccountRepository SystemAccountRepository, IMapper mapper)
        : BaseService<SystemAccount, short>(SystemAccountRepository), ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository = SystemAccountRepository;
        public async Task<AccountDetailResponse> login(LoginRequest request)
        {
            var account = await _systemAccountRepository.Login(request.AccountEmail, request.AccountPassword);
            return new AccountDetailResponse
            {
                AccountId = account.AccountId,
                AccountEmail = account.AccountEmail,
                AccountName = account.AccountName,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword
            };
        }

        public async Task<AccountDetailResponse> UpdateAccount(UpdateAccountRequest request, bool isAdmin)
        {
            var account = await _systemAccountRepository.GetByIdAsync(request.AccountId);
            if (account != null)
            {
                if (request.AccountName != null && account.AccountName != request.AccountName)
                    account.AccountName = request.AccountName;

                if (request.AccountPassword != null && account.AccountPassword != request.AccountPassword)
                    account.AccountPassword = request.AccountPassword;

                if (request.AccountEmail != null && request.AccountEmail != account.AccountEmail)
                {
                    // Check if the new email already exists
                    var existingAccount = await _systemAccountRepository.GetByEmailAsync(request.AccountEmail);
                    if (existingAccount != null)
                    {
                        throw new Exception("Email already exists.");
                    }
                }

                if (isAdmin && request.AccountRole.HasValue)
                    account.AccountRole = request.AccountRole;

                await _systemAccountRepository.UpdateAsync(account);

                return new AccountDetailResponse
                {
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountRole = account.AccountRole,
                    AccountPassword = account.AccountPassword
                };
            }
            return null;
        }

        public async Task<AccountDetailResponse> GetAccountById(short id)
        {
            var account = await _systemAccountRepository.GetByIdAsync(id);
            if (account != null)
            {
                return new AccountDetailResponse
                {
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountRole = account.AccountRole,
                    AccountPassword = account.AccountPassword
                };
            }
            return null;
        }

        public async Task<AccountListResponse> GetAllAccounts()
        {
            var accounts = await _systemAccountRepository.GetAllAsync();

            var response = new AccountListResponse
            {
                Accounts = accounts.Select(account => new AccountDetailResponse
                {
                    AccountId = account.AccountId,
                    AccountEmail = account.AccountEmail,
                    AccountName = account.AccountName,
                    AccountRole = account.AccountRole
                }).ToList()
            };

            return response;
        }

    }
}
