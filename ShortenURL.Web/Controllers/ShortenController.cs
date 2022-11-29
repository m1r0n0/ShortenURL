using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ShortenURL.Models;
using System.Diagnostics;

namespace ShortenURL.Controllers
{
    public class ShortenController : Controller
    {
        private readonly ShortenService _shortenService;
        private readonly IMapper _mapper;

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
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyLinks(MyLinksViewModel model)
        {
            LinkViewModelDTO linkViewModelDTO = new LinkViewModelDTO(model.UrlList);
            linkViewModelDTO = await _shortenService.MyLinksGet(linkViewModelDTO);
            model = _mapper.Map<MyLinksViewModel>(linkViewModelDTO);
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
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}