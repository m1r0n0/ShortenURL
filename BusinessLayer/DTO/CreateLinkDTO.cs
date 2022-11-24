using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTO
{
    class CreateLinkDTO
    {
		public IList<Url> Url { get; set; } = default!;

		public string FullUrl { get; set; } = string.Empty;
		public string ShortUrl { get; set; } = string.Empty;
		public bool IsPrivate { get; set; }
	}
}
