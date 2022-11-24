using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using ShortenURL.Models;

namespace BusinessLayer.Services
{
    internal class ShortenService
    {
        string shortened = "https://shrtUrl/";
        string userEmail = string.Empty;
        bool isThereSimilar = true;
        int key;
        private readonly DataAccessLayer.Data.ApplicationContext _context;

        Random Rand = new Random();
        public Url UrlObj { get; set; }

        public ShortenService(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }

        public async Task<CreateLinkViewModel> CreateLinkPost(CreateLinkViewModel model, System.Security.Principal.IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                userEmail = user.Identity.Name;
            }

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
            UrlObj = new Url { UserEmail = userEmail, FullUrl = model.FullUrl, ShortUrl = shortened, IsPrivate = model.IsPrivate };
            _context.Url.Add(UrlObj);
            await _context.SaveChangesAsync();
            model.ShortUrl = shortened;
            return model;
        }
    }
}
