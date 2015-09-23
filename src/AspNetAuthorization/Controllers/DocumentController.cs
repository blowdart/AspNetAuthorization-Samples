using System.Threading.Tasks;
using AspNetAuthorization.Authorization;
using AspNetAuthorization.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (await authorizationService.AuthorizeAsync(User, new Document(), Operations.Create))
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
