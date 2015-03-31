using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.Authorization;

using AspNetAuthorization.Models;
using System.Security.Claims;
using AspNetAuthorization.Authorization;

namespace AspNetAuthorization.Authorization
{
    public class DocumentAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Document>
    {
        public override void Handle(AuthorizationContext context, OperationAuthorizationRequirement requirement, Document resource)
        {
            var isSuperUser = context.User.FindFirst(c => c.Type == "Superuser" && c.Issuer == "urn:idunno.org" && c.Value == "True");
            if (isSuperUser != null)
            {
                context.Succeed(requirement);
                return;
            }

            var documentPermissionClaim = context.User.FindFirst(c => c.Type == "Documents" && c.Issuer == "urn:idunno.org");

            if (documentPermissionClaim == null)
            {
                context.Fail();
                return;
            }

            if (MapClaimsToOperations(documentPermissionClaim.Value).Contains(requirement))
            {
                context.Succeed(requirement);
            }
        }

        private IEnumerable<OperationAuthorizationRequirement> MapClaimsToOperations(string claimValue)
        {
            var mappedPermissions = new List<OperationAuthorizationRequirement>();

            if (claimValue.Contains('C'))
            {
                mappedPermissions.Add(Operations.Create);
            }
            if (claimValue.Contains('R'))
            {
                mappedPermissions.Add(Operations.Read);
            }
            if (claimValue.Contains('U'))
            {
                mappedPermissions.Add(Operations.Update);
            }
            if (claimValue.Contains('D'))
            {
                mappedPermissions.Add(Operations.Delete);
            }

            return mappedPermissions;
        }
    }
}