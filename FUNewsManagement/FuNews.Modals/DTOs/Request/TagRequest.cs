namespace FUNews.Modals.DTOs.Request
{
    public class TagRequest
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}