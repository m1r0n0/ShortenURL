using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    [Index("UserId", "ShortUrl")]
    public class Url
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
