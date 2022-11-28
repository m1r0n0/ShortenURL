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

        ShortenService shortenService = new ShortenService();

        private void IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                shortenService.GiveUserID(User.Identity.Name);
            }
        }

        [HttpGet]
        public IActionResult CreateLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(CreateLinkViewModel model)
        {
            LinkViewModel_DTO linkViewModel_DTO = new LinkViewModel_DTO(model.FullUrl, model.ShortUrl, model.IsPrivate);
            IsAuthenticated();
            linkViewModel_DTO = await shortenService.CreateLinkPost(linkViewModel_DTO);
            model = new CreateLinkViewModel (linkViewModel_DTO.FullUrl, linkViewModel_DTO.ShortUrl, linkViewModel_DTO.IsPrivate);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLinks(MyLinksViewModel model)
        {
            LinkViewModel_DTO linkViewModel_DTO = new LinkViewModel_DTO(model.UrlList);
            linkViewModel_DTO = await shortenService.MyLinksGet(linkViewModel_DTO);
            model = new MyLinksViewModel(linkViewModel_DTO.UrlList);
            return View(model);
        }

        [HttpGet]
        public IActionResult UseLink()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UseLink(UseLinkViewModel model)
        {
            LinkViewModel_DTO linkViewModel_DTO = new LinkViewModel_DTO(model.FullUrl, model.ShortUrl, model.IsPrivate);
            IsAuthenticated();
            linkViewModel_DTO = shortenService.UseLinkPost(linkViewModel_DTO);
            model = new UseLinkViewModel(linkViewModel_DTO.FullUrl, linkViewModel_DTO.ShortUrl, linkViewModel_DTO.IsPrivate);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}