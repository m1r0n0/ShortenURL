namespace BusinessLayer.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string FullUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
