using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
                context.User.Claims.FirstOrDefault(c => c.Type == "HairColour" && c.Issuer == Issuers.Idunno);

            if (hairColour == null)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}    