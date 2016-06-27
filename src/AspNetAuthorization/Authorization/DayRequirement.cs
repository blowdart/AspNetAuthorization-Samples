using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AspNetAuthorization.Authorization
{
    // THIS IS BAD.
    // It doesn't work as you expect, and is a deliberate bad example.
    // If you're writing an authorization requirement which doesn't require 
    // something from an identity it's likely NOT an authorization requirement.

    public class DayRequirement : AuthorizationHandler<DayRequirement>, IAuthorizationRequirement
    {
        public DayRequirement(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        protected DayOfWeek DayOfWeek { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DayRequirement requirement)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek)
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
