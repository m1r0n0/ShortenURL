using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ShortenURL.Models;
using System.Diagnostics;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ShortenURL.Web.Controllers;
using Microsoft.AspNetCore.Http;

namespace ShortenURL.Controllers
{
    public class ShortenController : AppController
    {
        private readonly IShortenService _shortenService;
        private readonly IMapper _mapper;

        public ShortenController(IHttpContextAccessor httpContextAccessor, IShortenService shortenService, IMapper mapper) : base(httpContextAccessor)
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
            linkViewModelDTO = await _shortenService.CreateShortLinkFromFullUrl(linkViewModelDTO, GetUserIdFromClaims());
            model = _mapper.Map<CreateLinkViewModel>(linkViewModelDTO);          
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult MyLinks()
        {
            LinkViewModelDTO linkViewModelDTO = null;
            linkViewModelDTO = _shortenService.GetURLsForCurrentUser(GetUserIdFromClaims());
            MyLinksViewModel model = _mapper.Map<MyLinksViewModel>(linkViewModelDTO);
            return View(model);
        }

        [HttpPatch]
        [Authorize]
        public IActionResult ChangeLinkPrivacy(int id, bool state)
        {
            _shortenService.ChangePrivacy(id, state);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}