using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AspNetAuthorization.Controllers
{
    public class AccountController : Controller
    {
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
                claims.Add(new Claim(ClaimTypes.Name, selectedIdentity, ClaimValueTypes.String, Issuers.Microsoft));

                switch (selectedIdentity)
                {
                    case "adam":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(2000, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimNames.BadgeNumber, "12345", ClaimValueTypes.String, Issuers.Contoso));
                        break;
                    case "barry":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1970, 06, 08).ToString("u"), ClaimValueTypes.DateTime, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimTypes.Role, "User", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim("CanWeFixIt", "YesWeCan", ClaimValueTypes.String, "urn:bobthebuilder.com"));
                        claims.Add(new Claim("Documents", "CRUD", ClaimValueTypes.String, Issuers.Idunno));
                        claims.Add(new Claim("HairColour", "Brown", ClaimValueTypes.String, Issuers.Idunno));
                        claims.Add(new Claim(ClaimNames.BadgeNumber, "12345", ClaimValueTypes.String, Issuers.Contoso));
                        break;
                    case "charlie":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim("CanWeFixIt", "NoWeCant", ClaimValueTypes.String, "urn:bobthebuilder.com"));
                        claims.Add(new Claim("Documents", "R", ClaimValueTypes.String, Issuers.Idunno));
                        claims.Add(new Claim(ClaimNames.VisitorPassExpiry, DateTime.Now.AddDays(1).ToString(), ClaimValueTypes.DateTime, Issuers.Contoso));
                        break;
                    case "david":
                        claims.Add(new Claim(ClaimTypes.DateOfBirth, new DateTime(1990, 01, 01).ToString("u"), ClaimValueTypes.DateTime, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimTypes.Role, "Guest", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim(ClaimNames.VisitorPassExpiry, DateTime.Now.AddDays(-1).ToString(), ClaimValueTypes.DateTime, Issuers.Contoso));
                        break;
                    case "gary":
                        claims.Add(new Claim(ClaimTypes.Role, "Guest", ClaimValueTypes.String, Issuers.Microsoft));
                        break;
                    case "plip":
                        claims.Add(new Claim(ClaimTypes.Role, "Guest", ClaimValueTypes.String, Issuers.Microsoft));
                        claims.Add(new Claim("HairColour", "Ginger", ClaimValueTypes.String, Issuers.Idunno));
                        break;
                    default:
                        break;
                }

                var identity = new ClaimsIdentity(claims, "sampleAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.Authentication.SignInAsync("Cookie", principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        IsPersistent = false,
                        AllowRefresh = false
                    });
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
