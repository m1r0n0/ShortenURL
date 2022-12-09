using Microsoft.AspNetCore.Mvc;

namespace ShortenURL.Web.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult PageNotFoundError()
        {
            return View();
        }
    }
}
