using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace PolicyProvider
{
    public class CustomAuthorizationRequirementHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        protected override void Handle(
            AuthorizationContext context, 
            CustomAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var matchingClaims =
                    context.User.Claims.Any(c => string.Equals(c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase)
                                            && string.Equals(c.Value, requirement.ClaimValue, StringComparison.Ordinal));

                if (matchingClaims)
                {
                    context.Succeed(requirement);
                }
            }            
        }
    }
}
