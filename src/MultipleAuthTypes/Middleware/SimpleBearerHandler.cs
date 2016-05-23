using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace MultipleAuthTypes.Middleware
{
    // DON'T DO THIS. IT MAKES ME CRY.
    public class SimpleBearerHandler : AuthenticationHandler<SimpleBearerOptions>
    {
        public SimpleBearerHandler()
        {
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            Response.StatusCode = 401;
            Response.Headers["WWW-Authenticate"] = "Bearer";
            return Task.FromResult(false);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var header = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                return Task.FromResult(AuthenticateResult.Skip()); 
            }

            var user = header.Substring(7);
            var principal = PrincipalFactory.Get(user);

            if (principal == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("No such user"));
            }

            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}