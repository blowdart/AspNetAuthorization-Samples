using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MultipleAuthTypes.Controllers
{
    [Route("api/[controller]")]
    public class IdentitiesController : Controller
    {
        // GET: api/identities
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return PrincipalFactory.UserNames;
        }

        // GET api/identites/adam
        [HttpGet("{name}")]
        public IEnumerable<ClaimsIdentity> Get(string name)
        {
            return PrincipalFactory.Get(name).Identities;
        }
    }
}
