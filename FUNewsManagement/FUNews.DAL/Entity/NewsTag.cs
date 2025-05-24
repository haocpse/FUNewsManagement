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
        public string NewsArticleID { get; set; } = string.Empty;
        public int TagID { get; set; }

        [ForeignKey(nameof(NewsArticleID))]
        public NewsArticle? NewsArticle { get; set; }

        [ForeignKey(nameof(TagID))]
        public Tag? Tag { get; set; }
    }
}
