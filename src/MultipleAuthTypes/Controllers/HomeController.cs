using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MultipleAuthTypes.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}