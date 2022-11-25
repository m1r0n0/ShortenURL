using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ShortenURL.Models;
using System.Diagnostics;

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
            ShortenService shortenService = new ShortenService();
            CreateLinkVM_DTO createLinkVM_DTO = new CreateLinkVM_DTO(model.FullUrl, model.ShortUrl, model.IsPrivate);

            if (User.Identity.IsAuthenticated)
            {
                shortenService.GiveUserID(User.Identity.Name);
            }          
            createLinkVM_DTO = await shortenService.CreateLinkPost(createLinkVM_DTO);
            model = new CreateLinkViewModel (createLinkVM_DTO.FullUrl, createLinkVM_DTO.ShortUrl, createLinkVM_DTO.IsPrivate);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLinks(MyLinksViewModel model)
        {
            /*if (_context.Url != null)
            {
                model.Url = await _context.Url.ToListAsync();
            }*/
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
            //put alternative code from CreateLink
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}