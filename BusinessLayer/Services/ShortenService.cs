using BusinessLayer.DTOs;
using DataAccessLayer.Models;
namespace BusinessLayer.Services
{
    public class ShortenService
    {
        string shortened = "https://shrtUrl/";
        string userId = string.Empty;
        bool isThereSimilar = true;
        int key;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        /*public ShortenService(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }*/

        public async Task<CreateLinkVM_DTO> CreateLinkPost(CreateLinkVM_DTO model)
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

                foreach (var item in _context.Url)
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
            UrlObj = new Url { UserId = userId, FullUrl = model.FullUrl, ShortUrl = shortened, IsPrivate = model.IsPrivate };
            _context.Url.Add(UrlObj);
            await _context.SaveChangesAsync();
            model.ShortUrl = shortened;
            return model;
        }

        public void GiveUserID(string _name)
        {
            foreach (var item in _context.User)
            {
                if (_name == item.UserName)
                {
                    userId = item.Id;
                }
            }
        }

        public async Task<CreateLinkVM_DTO> UseLinkPost(CreateLinkVM_DTO model)
        {
            foreach (var item in _context.Url)
            {
                if (item.IsPrivate)
                {
                    if (model.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserId == userId)
                        {
                            model.FullUrl = item.FullUrl;
                            break;
                        }
                        else
                        {
                            model.FullUrl = "You don't have acces to this link!";
                            break;
                        }
                    }
                }
                else
                {
                    if (model.ShortUrl == item.ShortUrl)
                    {
                        model.FullUrl = item.FullUrl;
                        break;
                    }

                }

            }
            return model;
        }


    }
}
