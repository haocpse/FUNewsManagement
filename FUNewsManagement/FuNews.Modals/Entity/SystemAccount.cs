using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Entity
{
    public class SystemAccount
    {
        [Key]
        public short AccountId { get; set; } 

        [StringLength(100)]
        public string? AccountName { get; set; }

        [StringLength(70)]
        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        [StringLength(70)]
        public string? AccountPassword { get; set; }

        // Navigation (optional)
        public ICollection<NewsArticle>? CreatedNewsArticles { get; set; }
        public ICollection<NewsArticle>? UpdatedNewsArticles { get; set; }

        public static class Roles
        {
            public const int Staff = 1;
            public const int Lecturer = 2;
            public const int Admin = 3;
        }
    }
}
