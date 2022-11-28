using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace BusinessLayer.DTOs
{
    public class LinkViewModel_DTO
    {
		public string FullUrl { get; set; } = string.Empty;
		public string ShortUrl { get; set; } = string.Empty;
		public bool IsPrivate { get; set; }
		public IList<Url> UrlList { get; set; } = default!;

        public LinkViewModel_DTO(string _fullUrl, string _shortUrl, bool _isPrivate)
		{
			FullUrl = _fullUrl;
			ShortUrl = _shortUrl;
			IsPrivate = _isPrivate;
		}
        public LinkViewModel_DTO(IList<Url> _urlList)
        {
			_urlList = UrlList;
        }
    }
}
