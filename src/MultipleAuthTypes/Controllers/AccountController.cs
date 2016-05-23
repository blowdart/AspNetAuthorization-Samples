using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MultipleAuthTypes.Controllers
{
    public class AccountController : Controller
    {
        private const string Issuer = "urn:microsoft.example";

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewData["Title"] = "Pick an Identity";
            return View("Unauthorized");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            ViewData["Title"] = "Forbidden";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Unauthorized(string returnUrl = null)            
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Title"] = "Unauthorized";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Unauthorized(string selectedIdentity = null, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(selectedIdentity))
            {
                await HttpContext.Authentication.SignOutAsync("Cookie");
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

                await HttpContext.Authentication.SignInAsync("Cookie", principal);
            }

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("Cookie");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private RouteValueDictionary ReturnUrlParameter(string returnUrl)
        {
            var rvd = new RouteValueDictionary();
            rvd.Add("returnUrl", returnUrl);
            return rvd;
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
