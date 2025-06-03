using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Response
{
    public class AccountResponse
    {
        public string? AccountEmail { get; set; }
        public string? AccountName { get; set; }
        public int? AccountRole { get; set; }
        public string? AccountPassword { get; set; }
        public short AccountId { get; set; }
    }
}
