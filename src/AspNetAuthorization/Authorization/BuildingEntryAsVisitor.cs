using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetAuthorization.Authorization
{
    public class BuildingEntryAsVisitor : AuthorizationHandler<EnterBuildingRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EnterBuildingRequirement requirement)
        {
            var expiryClaim =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimNames.VisitorPassExpiry &&
                                                        c.Issuer == Issuers.Contoso);

            if (expiryClaim == null)
            {
                return Task.FromResult(0);
            }

            DateTime expiryTime;

            if (DateTime.TryParse(expiryClaim.Value, out expiryTime))
            {
                if (expiryTime > DateTime.Now)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.FromResult(0);
        }
    }
}
