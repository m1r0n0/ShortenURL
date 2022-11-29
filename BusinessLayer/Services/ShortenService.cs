using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        public ShortenService(DataAccessLayer.Data.ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void GiveUserID(string _name)
        {
            foreach (var item in _context.UserList)
            {
                if (_name == item.UserName)
                {
                    _userId = item.Id;
                }
            }
        }

        public string GetUserID()
        {
            return _userId;
        }

        public async Task<LinkViewModelDTO> CreateLinkPost(LinkViewModelDTO modelDTO)
        {


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

            UrlObj = new Url { UserId = _userId, FullUrl = modelDTO.FullUrl, ShortUrl = _shortened, IsPrivate = modelDTO.IsPrivate };
            _context.UrlList.Add(UrlObj);
            await _context.SaveChangesAsync();
            modelDTO.ShortUrl = _shortened;
            return modelDTO;
        }

        public async Task<LinkViewModelDTO> MyLinksGet(LinkViewModelDTO model_DTO)
        {
            if (_context.UrlList != null)
            {
                model_DTO.UrlList = await _context.UrlList.ToListAsync();
            }
            return model_DTO;
        }

        public LinkViewModelDTO UseLinkPost(LinkViewModelDTO model_DTO)
        {
            foreach (var item in _context.UrlList)
            {
                if (item.IsPrivate)
                {
                    if (model_DTO.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserId == _userId)
                        {
                            model_DTO.FullUrl = item.FullUrl;
                            break;
                        }
                        else
                        {
                            model_DTO.FullUrl = "You don't have acces to this link!";
                            break;
                        }
                    }
                }
                else
                {
                    if (model_DTO.ShortUrl == item.ShortUrl)
                    {
                        model_DTO.FullUrl = item.FullUrl;
                        break;
                    }

                }

            }
            return model_DTO;
        }
    }
}
