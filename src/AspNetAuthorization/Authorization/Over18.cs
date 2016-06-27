using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AspNetAuthorization.Authorization
{
    public class Over18Requirement : AuthorizationHandler<Over18Requirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Over18Requirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.FromResult(0);
            }

            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (age >= 18)
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }

}