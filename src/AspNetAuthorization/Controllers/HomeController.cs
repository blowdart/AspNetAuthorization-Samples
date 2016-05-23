using Microsoft.AspNetCore.Mvc;

namespace AspNetAuthorization.Controllers
{
    public class HomeController : Controller
    {
        private const string Issuer = "urn:microsoft.example";

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bearer()
        {
            return View();
        }
    }
}