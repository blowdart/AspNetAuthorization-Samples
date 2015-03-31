using AspNetAuthorization.Authorization;
using AspNetAuthorization.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetAuthorization.Controllers
{
    [Authorize(Policy = "Documents")]
    public class DocumentController : Controller
    {
        IAuthorizationService authorizationService;

        public DocumentController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        // GET: /<controller>/=
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            if (authorizationService.Authorize(User, new Document(), Operations.Create))
            {
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(403);
            }
        }

    }
}
