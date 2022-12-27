using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShortenURL.Web.Controllers
{

    public abstract class AppController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppController(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor; 
        }

        public string GetUserIdFromClaims()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
