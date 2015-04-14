using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace AspNetAuthorization.Controllers
{
    [Authorize("WebApi")]
    [ResponseCache(NoStore = true, Duration = 0, VaryByHeader = "Authorization")]
    [Route("api/[controller]")]
    public class WhoIsBearerController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            if (User.Identity.IsAuthenticated == false)
                return new List<string>() { "Anonymous" };
            else
            {
                return from i in User.Identities
                       from c in i.Claims
                       where c.Type == ClaimTypes.Name
                       select c.Value;
            }
        }
    }
}
