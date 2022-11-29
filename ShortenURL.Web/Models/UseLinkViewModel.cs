using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using DataAccessLayer.Models;

namespace ShortenURL.Models
{
    public class UseLinkViewModel
    {
        [Required]
        [Display(Name = "Your full URL")]
        public string FullUrl { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Your short URL")]
        public string ShortUrl { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Private link?")]
        public bool IsPrivate { get; set; }

        public IList<Url> UrlList { get; set; } = default!;

        public UseLinkViewModel(string _fullUrl, string _shortUrl, bool _isPrivate)
        {
            FullUrl = _fullUrl;
            ShortUrl = _shortUrl;
            IsPrivate = _isPrivate;
        }

        public UseLinkViewModel()
        {
        }
    }
}
