using BusinessLayer.DTOs;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class ShortenService
    {
        string shortened = "https://shrtUrl/";
        private string userId = string.Empty;
        private bool isThereSimilar = true;
        private int key;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        /*public ShortenService(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }*/
        public void GiveUserID(string _name)
        {
            foreach (var item in _context.UserList)
            {
                if (_name == item.UserName)
                {
                    userId = item.Id;
                }
            }
        }

        public async Task<LinkViewModel_DTO> CreateLinkPost(LinkViewModel_DTO model_DTO)
        {

            while (true)
            {
                shortened = "https://shrtUrl.com/";
                for (int i = 0; i < 4; i++)
                {
                    key = Rand.Next(1, 4);
                    switch (key)
                    {
                        case 1:
                            shortened += (char)Rand.Next(48, 58);
                            break;
                        case 2:
                            shortened += (char)Rand.Next(65, 91);
                            break;
                        case 3:
                            shortened += (char)Rand.Next(97, 123);
                            break;
                    }
                }

                foreach (var item in _context.UrlList)
                {
                    if (shortened == item.ShortUrl)
                    {
                        isThereSimilar = true;
                        break;
                    }
                    else
                    {
                        isThereSimilar = false;
                    }
                }

                if (!isThereSimilar)
                {
                    break;
                }
            }
            UrlObj = new Url { UserId = userId, FullUrl = model_DTO.FullUrl, ShortUrl = shortened, IsPrivate = model_DTO.IsPrivate };
            _context.UrlList.Add(UrlObj);
            await _context.SaveChangesAsync();
            model_DTO.ShortUrl = shortened;
            return model_DTO;
        }

        public async Task<LinkViewModel_DTO> MyLinksGet(LinkViewModel_DTO model_DTO)
        {
            if (_context.UrlList != null)
            {
                model_DTO.UrlList = await _context.UrlList.ToListAsync();
            }
            return model_DTO;
        }

        public LinkViewModel_DTO UseLinkPost(LinkViewModel_DTO model_DTO)
        {
            foreach (var item in _context.UrlList)
            {
                if (item.IsPrivate)
                {
                    if (model_DTO.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserId == userId)
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
