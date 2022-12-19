using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace BusinessLayer.Services
{
    public class ShortenService : IShortenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        Random Rand = new Random();
        private Url UrlObj { get; set; }

        public ShortenService(DataAccessLayer.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GetUserIDFromUserName(string name)
        {
            var r = _context.UserList.Where(x => x.UserName.Equals(name)).FirstOrDefault();
            if (r == null)
            {
                return string.Empty;
            } else
            {
                return r.Id;
            }
        }

        public async Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName)
        {
            string _shortened = string.Empty;
            bool _isThereSimilar = false;
            int _key;
            modelDTO.UserId = GetUserIDFromUserName(userName);
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
            UrlObj = new Url { UserId = GetUserIDFromUserName(userName), FullUrl = modelDTO.FullUrl, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(UrlObj);
            await _context.SaveChangesAsync();
            UrlObj.ShortUrl = IdToShortURL(UrlObj.Id);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = _configuration["shortenedBegining"] + UrlObj.ShortUrl;
            return modelDTO;
        }

        public LinkViewModelDTO GetURLsForCurrentUser(LinkViewModelDTO modelDTO, string userName)
        {
            if (_context.UrlList != null)
            {
                modelDTO.UrlList = _context.UrlList.Where(i => i.UserId == GetUserIDFromUserName(userName)).ToList();
            }
            return modelDTO;
        }

        public string IdToShortURL(int n)
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
            return ReverseString(shorturl);
        }

        public string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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
