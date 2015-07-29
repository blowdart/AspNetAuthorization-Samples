using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace MultipleAuthTypes
{
    public class AllowAnonymousUsersRequirement : AuthorizationHandler<AllowAnonymousUsersRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, AllowAnonymousUsersRequirement requirement)
        {
            context.Succeed(requirement);
        }
    }
}
