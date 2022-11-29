using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ShortenURL.Models;
using System.Diagnostics;
using DataAccessLayer.Data;

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

        private readonly ShortenService _shortenService;
        private readonly IMapper _mapper;
        //private readonly DataAccessLayer.Data.ApplicationContext _context;

        public ShortenController(ShortenService shortenService, IMapper mapper)
        {
            _shortenService = shortenService;
            _mapper = mapper;
        }

        private void IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                _shortenService.GiveUserID(User.Identity.Name);
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
            LinkViewModelDTO linkViewModelDTO = new LinkViewModelDTO(model.FullUrl, model.ShortUrl, model.IsPrivate);
            IsAuthenticated();
            linkViewModelDTO = await _shortenService.CreateLinkPost(linkViewModelDTO);
            model = _mapper.Map<CreateLinkViewModel>(linkViewModelDTO);
            //model = new CreateLinkViewModel (linkViewModel_DTO.FullUrl, linkViewModel_DTO.ShortUrl, linkViewModel_DTO.IsPrivate);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLinks(MyLinksViewModel model)
        {
            LinkViewModelDTO linkViewModelDTO = new LinkViewModelDTO(model.UrlList);
            linkViewModelDTO = await _shortenService.MyLinksGet(linkViewModelDTO);
            model = _mapper.Map<MyLinksViewModel>(linkViewModelDTO);
            //model = new MyLinksViewModel(linkViewModel_DTO.UrlList);
            IsAuthenticated();
            model.UserId = _shortenService.GetUserId();
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
            LinkViewModelDTO linkViewModelDTO = new LinkViewModelDTO(model.FullUrl, model.ShortUrl, model.IsPrivate);
            IsAuthenticated();
            linkViewModelDTO = _shortenService.UseLinkPost(linkViewModelDTO);
            model = _mapper.Map<UseLinkViewModel>(linkViewModelDTO);
            //model = new UseLinkViewModel(linkViewModel_DTO.FullUrl, linkViewModel_DTO.ShortUrl, linkViewModel_DTO.IsPrivate);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}