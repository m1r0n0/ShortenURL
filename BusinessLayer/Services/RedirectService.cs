using BusinessLayer.Interfaces;
using DataAccessLayer.Data;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class RedirectService : IRedirectService
    {
        private string _userId = string.Empty;
        private string _fullUrl = string.Empty;
        private string _checkHttp = string.Empty;
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
            if (shortUrl != null)
            {
                shortUrl = _configuration["shortenedBegining"] + shortUrl;
                _userId = _shortenService.GetUserIDFromUserName(userName);
                foreach (var item in _context.UrlList)
                {
                    if (item.IsPrivate)
                    {
                        if (shortUrl == item.ShortUrl)
                        {
                            if (item.UserId == _userId)
                            {
                                _fullUrl = item.FullUrl;
                                break;
                            }
                            else
                            {
                                _fullUrl = "You don't have acces to this link!";
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (shortUrl == item.ShortUrl)
                        {
                            _fullUrl = item.FullUrl;
                            break;
                        }
                    }
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
                return "https://shorturl.com" + _configuration["port"] + "/Home/Index";//IS IT LIKE THAT?    //"Home/Index"; 
            }
        }
    }
}
