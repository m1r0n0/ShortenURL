using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Services;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using BusinessLayer.Interfaces;

namespace ShortenURL.Web.Controllers
{
    public class RedirectController : Controller
    {
        private readonly IRedirectService _redirectService;
        private readonly IMapper _mapper;

        public RedirectController(IRedirectService redirectService, IMapper mapper)
        {
            _redirectService = redirectService;
            _mapper = mapper;
        }


        [Route("{id?}")]
        public IActionResult DoRedirect(string? id)
        {
            return Redirect(_redirectService.GetLinkToRedirect(id, User.Identity.Name));
        }
    }
}
