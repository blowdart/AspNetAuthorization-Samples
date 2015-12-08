using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;

namespace MultipleAuthTypes.Middleware
{
    // DON'T DO THIS. IT MAKES ME CRY.
    public class SimpleBearerHandler : AuthenticationHandler<SimpleBearerOptions>
    {
        public SimpleBearerHandler()
        {
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            Response.StatusCode = 401;
            Response.Headers["WWW-Authenticate"] = "Bearer";
            return false;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var header = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                return null;
            }

            var user = header.Substring(7);
            var principal = PrincipalFactory.Get(user);

            if (principal == null)
            {
                return AuthenticateResult.Failed("No such user");
            }

            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
        }
    }
}