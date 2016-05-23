using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MultipleAuthTypes.Models;

namespace MultipleAuthTypes.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Cookie,Bearer")]
    [ResponseCache(NoStore = true, Duration = 0, VaryByHeader = "Authorization")]
    [Route("api/[controller]")]
    public class WhoAmIController : Controller
    {
        // GET: api/values
        [HttpGet]
        [AllowAnonymous]
        public BearerIdentity Get()
        {
            var identity = new BearerIdentity();

            identity.IsAuthenticated = User.Identity.IsAuthenticated;

            var names = new List<string>();

            if (User.Identity.IsAuthenticated == false)
            {
                names.Add("-Anonymous-");
            }
            else
            {
                names.AddRange(from i in User.Identities
                               from c in i.Claims
                               where c.Type == ClaimTypes.Name
                               select c.Value);
            }

            identity.Names = names.ToArray();

            return identity;
        }
    }
}
