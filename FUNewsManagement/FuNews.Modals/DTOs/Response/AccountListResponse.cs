using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Response
{
    public class AccountListResponse
    {
        public List<AccountDetailResponse> Accounts { get; set; } = new List<AccountDetailResponse>();
    }
}
