using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Models;
using ShortenURL.Models;
using System.Diagnostics;
using ShortenURL.Web.Models;

namespace ShortenURL.Controllers
{
    public class ShortenController : Controller
    {
        string shortened = "https://shrtUrl/";
        string userEmail = string.Empty;
        Random Rand = new Random();
        bool isThereSimilar = true;
        int key;
        private readonly BusinessLayer.Data.ApplicationContext _context;

        [BindProperty]
        public Url UrlObj { get; set; }

        public ShortenController(BusinessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> CreateLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(CreateLinkViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                userEmail = User.Identity.Name;
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
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLinks(MyLinksViewModel model)
        {
            if (_context.Url != null)
            {
                model.Url = await _context.Url.ToListAsync();
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UseLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UseLink(UseLinkViewModel model)
        {
            foreach (var item in _context.Url)
            {
                if (item.IsPrivate)
                {
                    if (model.ShortUrl == item.ShortUrl)
                    {
                        if (item.UserEmail == User.Identity.Name) 
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
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}