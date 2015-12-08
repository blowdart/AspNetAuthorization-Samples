
using System;
using System.Linq;
using Microsoft.AspNet.Authorization;

namespace AspNetAuthorization.Authorization
{
    public class NoGingersAuthorizationHandler : AuthorizationHandler<NoGingersRequirement>
    {
        protected override void Handle(AuthorizationContext context, NoGingersRequirement requirement)
        {
            var hairColour =
                context.User.Claims.FirstOrDefault(c => c.Type == "HairColour" && c.Issuer == "urn:idunno.org");

            if (hairColour == null || hairColour.Value != "ginger")
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
