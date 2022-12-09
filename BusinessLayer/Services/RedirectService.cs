using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class RedirectService : IRedirectService
    {
        private readonly IShortenService _shortenService;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public RedirectService(IShortenService shortenService, ApplicationContext applicationContext, IConfiguration configuration)
        {
            _shortenService = shortenService;
            _context = applicationContext;
            _configuration = configuration;
        }
        public string GetLinkToRedirect(string shortUrl, string userName)
        {
            string _fullUrl = string.Empty;
            string _checkHttp = string.Empty;
            if (shortUrl != null)
            {
                shortUrl = _configuration["shortenedBegining"] + shortUrl;
                var appropriateShortLink = _context.UrlList.Where(x => x.ShortUrl.Equals(shortUrl)).FirstOrDefault();
                if (appropriateShortLink != null)
                {
                    if (appropriateShortLink.IsPrivate)
                    {
                        if (appropriateShortLink.UserId == _shortenService.GetUserIDFromUserName(userName))
                        {
                            _fullUrl = appropriateShortLink.FullUrl;
                        }
                        else
                        {
                            _fullUrl = "https://shorturl.com" + _configuration["port"] + "/Errors/PageNotFoundError";//"You don't have acces to this link!";
                        }
                    }
                    else
                    {
                        _fullUrl = appropriateShortLink.FullUrl;
                    }
                }
                else
                {
                    _fullUrl = "https://shorturl.com" + _configuration["port"] + "/Errors/PageNotFoundError";
                }

                if (_fullUrl != string.Empty)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        _checkHttp += _fullUrl[i];
                    }
                    if ((_checkHttp != "http://") && (_checkHttp != "https:/"))
                    {
                        _fullUrl = "https://" + _fullUrl;
                    }
                }
                return _fullUrl;
            }
            else
            {
                return "https://shorturl.com" + _configuration["port"] + "/Home/Index";
            }
        }
    }
}
