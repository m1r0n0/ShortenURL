using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace BusinessLayer.DTOs
{
    public class CreateLinkVM_DTO
    {
		public IList<Url> Url { get; set; } = default!;

		public string FullUrl { get; set; } = string.Empty;
		public string ShortUrl { get; set; } = string.Empty;
		public bool IsPrivate { get; set; }

		public CreateLinkVM_DTO(string _fullUrl, string _shortUrl, bool _isPrivate)
		{
			FullUrl = _fullUrl;
			ShortUrl = _shortUrl;
			IsPrivate = _isPrivate;
		}
    }
}
