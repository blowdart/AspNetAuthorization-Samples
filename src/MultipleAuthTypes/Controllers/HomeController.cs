using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

namespace MultipleAuthTypes.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Cookie,Bearer")]
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}