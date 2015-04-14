using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.Framework.Logging;

namespace AspNetAuthorization.Middleware
{
    // DON'T DO THIS. IT MAKES ME CRY.
    public class SimpleBearerHandler : AuthenticationHandler<SimpleBearerOptions>
    {
        public SimpleBearerHandler()
        {
        }

        protected override void ApplyResponseChallenge()
        {
            if (Response.StatusCode != 401)
            {
                return;
            }

            Response.Headers["WWW-Authenticate"] = "Bearer";
        }

        protected override void ApplyResponseGrant()
        {
            // No SignIn / Signout support.
        }

        protected override AuthenticationTicket AuthenticateCore()
        {
            var header = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                return null;
            }

            var user = header.Substring(7);
            var principal = PrincipalFactory.Get(user);

            if (principal == null)
            {
                return null;
            }

            return new AuthenticationTicket(new ClaimsPrincipal(principal), new AuthenticationProperties(), "Bearer");
        }
    }
}