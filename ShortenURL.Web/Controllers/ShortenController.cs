using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using ShortenURL.Models;
using System.Diagnostics;
using ShortenURL.Web.Models;

namespace ShortenURL.Controllers
{
    public class ShortenController : Controller
    {
        /*string shortened = "https://shrtUrl/";
        string userEmail = string.Empty;
        Random Rand = new Random();
        bool isThereSimilar = true;
        int key;
        private readonly DataAccessLayer.Data.ApplicationContext _context;*/

        /*[BindProperty]
        public Url UrlObj { get; set; }*/

        /*public ShortenController(DataAccessLayer.Data.ApplicationContext context)
        {
            _context = context;
        }*/

        [HttpGet]
        public IActionResult CreateLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(CreateLinkViewModel model)
        {
            ShortenService.CreateLinkPost(model, User);
            //userEmail = User.Identity.Name;
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