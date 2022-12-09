using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class ShortenService : IShortenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        public ShortenService(DataAccessLayer.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GetUserIDFromUserName(string name)
        {
            var r = _context.UserList.Where(x => x.UserName.Equals(name)).FirstOrDefault();
            return r.Id;
        }

        public async Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName)
        {
            string _shortened = string.Empty;
            bool _isThereSimilar = false;
            int _key;
            modelDTO.UserId = GetUserIDFromUserName(userName);
            while (true)
            {
                _shortened = _configuration["shortenedBegining"];
                for (int i = 0; i < 4; i++)
                {
                    _key = Rand.Next(1, 4);
                    switch (_key)
                    {
                        case 1:
                            _shortened += (char)Rand.Next(48, 58);
                            break;
                        case 2:
                            _shortened += (char)Rand.Next(65, 91);
                            break;
                        case 3:
                            _shortened += (char)Rand.Next(97, 123);
                            break;
                    }
                }

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
            UrlObj = new Url { UserId = GetUserIDFromUserName(userName), FullUrl = modelDTO.FullUrl, ShortUrl = _shortened, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(UrlObj);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = _shortened;
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

        public string EncodeUrl (int urlId)
        {
            string base62String = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ";
            string hashString = "";
            while (urlId > 0) 
            {
                hashString += base62String[urlId % 62];
                urlId /= 62;
            }
            return hashString;
        }
    }
}
