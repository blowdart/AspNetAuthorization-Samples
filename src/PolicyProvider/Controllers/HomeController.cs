using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PolicyProvider.Controllers
{
    public class HomeController : Controller
    {
        private IAuthorizationService _authorizationService;

        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AnyAuthorized()
        {
            return View();
        }

        [Authorize(Policy = "RequireNameClaim")]
        public IActionResult RequireNameClaim()
        {
            return View();
        }


        [Authorize]
        //[Authorize(Policy = "custom|policy")]
        public async Task<IActionResult> CustomPolicy()
        {
            // Darned bugs. Right now you can't use a custom policy provider in the
            // authorize attribute, it only works in imperative checks. This will
            // be fixed for RTM.

            if (await _authorizationService.AuthorizeAsync(User, "custom|policy"))
            {
                return View();
            }
            else
            {
                return new ChallengeResult();
            }
        }
    }
}
