using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AspNetAuthorization.Authorization
{
    public class BaldAuthorizationHandler : AuthorizationHandler<NoGingersRequirement>
    {
        ILogger _logger;

        public BaldAuthorizationHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NoGingersRequirement requirement)
        {
            _logger.LogInformation("Checking for baldies.");

            var hairColour =
                context.User.Claims.FirstOrDefault(c => c.Type == "HairColour" && c.Issuer == Issuers.Idunno);

            if (hairColour == null)
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}    