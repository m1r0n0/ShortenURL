using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;

namespace ShortenURL.Models
{
    public class MyLinksViewModel
    {
        public IList<Url> Url { get; set; } = default!;
    }
}
