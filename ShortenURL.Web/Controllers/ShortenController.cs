using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ShortenURL.Models;
using System.Diagnostics;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ShortenURL.Controllers
{
    public class ShortenController : Controller
    {
        private readonly IShortenService _shortenService;
        private readonly IMapper _mapper;

        public ShortenController(IShortenService shortenService, IMapper mapper)
        {
            _shortenService = shortenService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLink(CreateLinkViewModel model)
        {
            LinkViewModelDTO linkViewModelDTO = _mapper.Map<LinkViewModelDTO>(model);
            linkViewModelDTO = await _shortenService.CreateShortLinkFromFullUrl(linkViewModelDTO);
            model = _mapper.Map<CreateLinkViewModel>(linkViewModelDTO);          
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyLinks(MyLinksViewModel model)
        {
            LinkViewModelDTO linkViewModelDTO = _mapper.Map<LinkViewModelDTO>(model);
            linkViewModelDTO = _shortenService.GetURLsForCurrentUser(linkViewModelDTO);
            model = _mapper.Map<MyLinksViewModel>(linkViewModelDTO);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}