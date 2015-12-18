using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;

namespace AspNetAuthorization.Authorization
{
    public class BaldAuthorizationHandler : AuthorizationHandler<NoGingersRequirement>
    {
        ILogger _logger;

        public BaldAuthorizationHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);

        }

        protected override void Handle(AuthorizationContext context, NoGingersRequirement requirement)
        {
            _logger.LogInformation("Checking for baldies.");

            var hairColour =
                context.User.Claims.FirstOrDefault(c => c.Type == "HairColour" && c.Issuer == "urn:idunno.org");

            if (hairColour == null)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}    