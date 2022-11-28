using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace ShortenURL.Models
{
    public class MyLinksViewModel
    {
        public IList<Url> UrlList { get; set; } = default!;

        public MyLinksViewModel(IList<Url> _urlList)
            {
            _urlList = UrlList;
            }
    }
}
