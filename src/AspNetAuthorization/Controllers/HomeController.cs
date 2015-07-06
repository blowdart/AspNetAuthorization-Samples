using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNet.Mvc;

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PickIdentity(string returnUrl = null, bool unauthorized = false, bool forbidden = false)
        {
            return View();
        }

        public IActionResult Bearer()
        {
            return View();
        }
    }
}