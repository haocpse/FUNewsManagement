using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace FUNews.DAL.Repository
{
    public class SystemAccountRepository(FUNewsDbContext context) : BaseRepository<SystemAccount, short>(context), ISystemAccountRepository
    {
        public async Task<SystemAccount> Login(String email, String password)
        {
            var account = await _context.SystemAccounts
                .Where(a => a.AccountEmail == email && a.AccountPassword == password)
                .FirstOrDefaultAsync();
            if (account != null)
            {
                return account;
            }
            return null;
        }

        public async Task<SystemAccount> GetByEmailAsync(string email)
        {
            var account = await _context.SystemAccounts
                .Where(a => a.AccountEmail == email)
                .FirstOrDefaultAsync();
            return account;
        }
    }
}
