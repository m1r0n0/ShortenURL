using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShortenURL.Web.Controllers
{
    public class ErrorsController : AppController
    {
        public ErrorsController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public IActionResult PageNotFoundError()
        {
            return View();
        }
    }
}
