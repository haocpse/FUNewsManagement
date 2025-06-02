using FUNews.Modals.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Response.News
{
    public class NewsResponse
    {
        public string? NewsTitle { get; set; }
        public string? Headline { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public CategoryResponse? Category { get; set; }
        public bool? NewsStatus { get; set; }
        public List<TagResponse>? Tags { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
