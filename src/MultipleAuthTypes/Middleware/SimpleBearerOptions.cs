using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Authentication;

namespace MultipleAuthTypes.Middleware
{
    public class SimpleBearerOptions : AuthenticationOptions
    {
        public SimpleBearerOptions()
        {
            AuthenticationScheme = "Bearer";
            AutomaticAuthentication = false;
        }

        public IDictionary<string, ClaimsPrincipal> IdentityMap { get; set; }
    }
}