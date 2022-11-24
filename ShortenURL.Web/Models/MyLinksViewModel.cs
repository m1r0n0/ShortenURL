using Microsoft.EntityFrameworkCore;
using BusinessLayer.Models;

namespace ShortenURL.Models
{
    public class MyLinksViewModel
    {
        public IList<Url> Url { get; set; } = default!;
    }
}
