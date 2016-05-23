using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AspNetAuthorization.Controllers
{
    public class ProtectedController : Controller
    {
        IAuthorizationService authorizationService;

        public ProtectedController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public IActionResult Anyone()
        {
            return View();
        }

        [Authorize]
        public IActionResult AnyAuthorized()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AdministratorsOnly()
        {
            return View();
        }

        // Comma delimited role list is an OR
        [Authorize(Roles = "Administrator,User")]
        public IActionResult UserOrAdministrator()
        {
            return View();
        }
        
        // Seperate attributes AND
        [Authorize(Roles = "Administrator")]
        [Authorize(Roles = "User")]
        public IActionResult UserAndAdministrator()
        {
            return View();
        }


        [Authorize(Policy = "RequireBobTheBuilder")]
        public IActionResult AnyBuilder()
        {
            return View();
        }

        [Authorize(Policy = "Over18")]
        public IActionResult Over18()
        {
            return View();
        }

        [Authorize(Policy = "Over21")]
        public IActionResult Over21()
        {
            return View();
        }

        [Authorize(Policy = "TacoTuesday")]
        public IActionResult TacoTuesday()
        {
            return View();
        }

        [Authorize(Policy = "TequillaTacoTuesday")]
        public IActionResult TequillaTacoTuesday()
        {
            return View();
        }

        [Authorize(Policy = "NoGingers")]
        public IActionResult NoGingers()
        {
            return View();
        }

        [Authorize(Policy = "CanEnterContosoBuilding")]
        public IActionResult BuildingEntry()
        {
            return View();
        }
    }
}

