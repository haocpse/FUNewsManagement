using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        public string AccountEmail { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string AccountPassword { get; set; }
    }
}
