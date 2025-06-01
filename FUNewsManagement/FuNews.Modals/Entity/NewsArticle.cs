using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL.Entity
{
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = string.Empty;

        [StringLength(400)]
        public string? NewsTitle { get; set; }

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }

        [StringLength(4000)]
        public string? NewsContent { get; set; }

        [StringLength(400)]
        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // ==== Navigation properties ====

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public SystemAccount? CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        public SystemAccount? UpdatedBy { get; set; }
        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
