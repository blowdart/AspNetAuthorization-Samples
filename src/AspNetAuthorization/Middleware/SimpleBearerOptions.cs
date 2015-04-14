using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Authentication;

namespace AspNetAuthorization.Middleware
{
    public class SimpleBearerOptions : AuthenticationOptions
    {
        public SimpleBearerOptions()
        {
            AuthenticationScheme = "Bearer";
            AutomaticAuthentication = true;
        }

        public IDictionary<string, ClaimsPrincipal> IdentityMap { get; set; }
    }
}