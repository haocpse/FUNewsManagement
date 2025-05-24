using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Entity
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [StringLength(50)]
        public string? TagName { get; set; }

        [StringLength(400)]
        public string? Note { get; set; }

        // Quan hệ n-n với NewsArticle
        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
