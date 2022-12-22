using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;

namespace BusinessLayer.Services
{
    public class ShortenService : IShortenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        Random Rand = new Random();

        public ShortenService(DataAccessLayer.Data.ApplicationContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO)
        {
            string _shortened = string.Empty;
            bool _isThereSimilar = false;
            int _key;
            modelDTO.UserId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            while (true)
            {
                var appropriateShortLink = _context.UrlList.Where(x => x.ShortUrl.Equals(_shortened)).FirstOrDefault();
                if (appropriateShortLink != null)
                {
                    if (_shortened == appropriateShortLink.ShortUrl)
                    {
                        _isThereSimilar = true;
                        break;
                    }
                    else
                    {
                        _isThereSimilar = false;
                    }
                }
                else
                    break;

                if (!_isThereSimilar)
                {
                    break;
                }
            }
            Url urlObj = new Url { UserId = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, FullUrl = modelDTO.FullUrl, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(urlObj);
            await _context.SaveChangesAsync();
            urlObj.ShortUrl = IdToShortURL(urlObj.Id);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = _configuration["shortenedBegining"] + urlObj.ShortUrl;
            return modelDTO;
        }

        public LinkViewModelDTO GetURLsForCurrentUser(LinkViewModelDTO modelDTO)
        {
            if (_context.UrlList != null)
            {
                var userIdFromUserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                modelDTO.UrlList = _context.UrlList.Where(i => i.UserId == userIdFromUserName).ToList();
                foreach (Url url in _context.UrlList)
                {
                    url.ShortUrl = _configuration["shortenedBegining"] + url.ShortUrl;
                }
            }
            return modelDTO;
        }

        public string? IdToShortURL(int n)
        {
            // Map to store 62 possible characters 
            char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            string shorturl = "";

            // Convert given integer id to a base 62 number 
            while (n > 0)
            {
                // use above map to store actual character 
                // in short url 
                shorturl += (map[n % 62]);
                n = n / 62;
            }
            // Reverse shortURL to complete base conversion 
            return new String(shorturl.ToCharArray().Reverse().ToArray()).ToString(); ;
        }

        // Function to get integer ID back from a short url 
        public int ShortURLToID(string shortURL)
        {
            int id = 0; // initialize result 

            // A simple base conversion logic 
            for (int i = 0; i < shortURL.Length; i++)
            {
                if ('a' <= shortURL[i] &&
                           shortURL[i] <= 'z')
                    id = id * 62 + shortURL[i] - 'a';
                if ('A' <= shortURL[i] &&
                           shortURL[i] <= 'Z')
                    id = id * 62 + shortURL[i] - 'A' + 26;
                if ('0' <= shortURL[i] &&
                           shortURL[i] <= '9')
                    id = id * 62 + shortURL[i] - '0' + 52;
            }
            return id;
        }
    }
}
