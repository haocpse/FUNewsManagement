using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Entity
{
    public class NewsTag
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public int TagId { get; set; }

        [ForeignKey(nameof(NewsArticleId))]
        public NewsArticle? NewsArticle { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tag? Tag { get; set; }
    }
}
