using Microsoft.AspNet.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetAuthorization.Authorization
{
    public class BuildingEntryAsEmployeeHandler : AuthorizationHandler<EnterBuildingRequirement>
    {
        IEmployeeRepository _employeeRepository;

        public BuildingEntryAsEmployeeHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        protected override void Handle(AuthorizationContext context, EnterBuildingRequirement requirement)
        {
            var badgeNumber =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimNames.BadgeNumber && 
                                                        c.Issuer == Issuers.Contoso);

            if (badgeNumber != null && _employeeRepository.IsStillEmployed(badgeNumber.Value))
            {
                context.Succeed(requirement);
            }
        }
    }
}
