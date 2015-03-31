using System;
using System.Collections.Generic;
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
        public IActionResult PickIdentity(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PickIdentity(string selectedIdentity = null, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(selectedIdentity))
            {
                Response.SignOut();
                return RedirectToAction("Index");
            }
            else
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, selectedIdentity, ClaimValueTypes.String, Issuer));

                switch (selectedIdentity)
                {
                    case "adam":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(2000, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuer));
                        claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, "urn:microsoft.com"));
                        break;
                    case "barry":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1970, 06, 08).ToString("u"), ClaimValueTypes.DateTime, Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuer));
                        claims.Add(new Claim("CanWeFixIt", "YesWeCan", ClaimValueTypes.String, "urn:bobthebuilder.com"));
                        claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, "urn:idunno.org"));
                        break;
                    case "charlie":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer));
                        claims.Add(new Claim("CanWeFixIt", "NoWeCant", ClaimValueTypes.String, "urn:bobthebuilder.com"));
                        claims.Add(new Claim("Documents", "R", ClaimValueTypes.String, "urn:idunno.org"));
                        break;
                    case "david":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuer));
                        claims.Add(new Claim(ClaimTypes.Role, "Guest", ClaimValueTypes.String, Issuer));
                        break;
                    default:
                        break;
                }

                var identity = new ClaimsIdentity(claims, "sampleAuth");
                var principal = new ClaimsPrincipal(identity);

                Response.SignIn("Cookie", principal);
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public IActionResult LogOff()
        {
            Response.SignOut();
            return RedirectToAction("Index");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}