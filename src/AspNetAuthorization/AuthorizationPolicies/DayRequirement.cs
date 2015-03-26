using System;
using Microsoft.AspNet.Authorization;

namespace AspNetAuthorization.AuthorizationPolicies
{
    public class DayRequirement : AuthorizationHandler<DayRequirement>, IAuthorizationRequirement
    {
        public DayRequirement(DayOfWeek dayOfWeek)
        {
            DayOfWeek = dayOfWeek;
        }

        protected DayOfWeek DayOfWeek { get; set; }

        public override void Handle(AuthorizationContext context, DayRequirement requirement)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek)
            {
                context.Succeed(requirement);
            }
        }
    }
}