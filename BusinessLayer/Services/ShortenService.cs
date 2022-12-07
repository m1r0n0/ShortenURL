using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using DataAccessLayer.Data;

namespace BusinessLayer.Services
{
    public class ShortenService : IShortenService
    {
        private readonly IConfiguration _configuration;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        private string _shortened = string.Empty;
        private string _userId = string.Empty;
        private bool _isThereSimilar = false;
        private int _key;
        private string _checkHttp = "";

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        public ShortenService(DataAccessLayer.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GetUserIDFromUserName(string name)
        {
            foreach (var item in _context.UserList)
            {
                if (name == item.UserName)
                {
                    _userId = item.Id;
                }
            }
            return _userId;
        }

        /*public string GetUserID()
        {
            return _userId;
        }*/

        public async Task<LinkViewModelDTO> CreateShortLinkFromFullUrl(LinkViewModelDTO modelDTO, string userName)
        {
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

                foreach (var item in _context.UrlList)
                {
                    if (_shortened == item.ShortUrl)
                    {
                        _isThereSimilar = true;
                        break;
                    }
                    else
                    {
                        _isThereSimilar = false;
                    }
                }

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
        public string GetLinkToRedirect(LinkViewModelDTO modelDTO, string userName)
        {
            modelDTO.UserId = GetUserIDFromUserName(userName);
            foreach (var item in _context.UrlList)
            {
                if (item.IsPrivate)
                {
                    if (modelDTO.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserId == _userId)
                        {
                            modelDTO.FullUrl = item.FullUrl;
                            break;
                        }
                        else
                        {
                            modelDTO.FullUrl = "You don't have acces to this link!";
                            break;
                        }
                    }
                }
                else
                {
                    if (modelDTO.ShortUrl == item.ShortUrl)
                    {
                        modelDTO.FullUrl = item.FullUrl;
                        break;
                    }
                }
            }

            for (int i = 0; i < 7; i++)
            {
                _checkHttp += modelDTO.FullUrl[i];
            }
            if ((_checkHttp != "http://") && (_checkHttp != "https:/"))
            {
                modelDTO.FullUrl = "https://" + modelDTO.FullUrl;
            }
            return modelDTO.FullUrl;
        }
        /*public LinkViewModelDTO FindAppropriateLinkInDB(LinkViewModelDTO modelDTO, string userName)
        {
            modelDTO.UserId = GetUserIDFromUserName(userName);
            foreach (var item in _context.UrlList)
            {
                if (item.IsPrivate)
                {
                    if (modelDTO.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserId == _userId)
                        {
                            modelDTO.FullUrl = item.FullUrl;
                            break;
                        }
                        else
                        {
                            modelDTO.FullUrl = "You don't have acces to this link!";
                            break;
                        }
                    }
                }
                else
                {
                    if (modelDTO.ShortUrl == item.ShortUrl)
                    {
                        modelDTO.FullUrl = item.FullUrl;
                        break;
                    }
                }
            }         
            return modelDTO;
        }*/
    }
}
