using System.ComponentModel.DataAnnotations;

namespace FUNews.Modals.DTOs.Response
{
    public class TagResponse
    {
        public int TagId { get; set; }

        public string? TagName { get; set; }

        public string? Note { get; set; }
    }
}