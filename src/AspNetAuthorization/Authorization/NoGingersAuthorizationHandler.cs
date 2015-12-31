
using System;
using System.Linq;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;

namespace AspNetAuthorization.Authorization
{
    public class NoGingersAuthorizationHandler : AuthorizationHandler<NoGingersRequirement>
    {
        ILogger _logger;

        public NoGingersAuthorizationHandler(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        protected override void Handle(AuthorizationContext context, NoGingersRequirement requirement)
        {
            _logger.LogInformation("Checking for soul free mutants.");

            var hairColour =
                context.User.Claims.FirstOrDefault(c => c.Type == "HairColour" && c.Issuer == "urn:idunno.org");

            if (hairColour != null &&
                (string.IsNullOrEmpty(hairColour.Value) || 
                 string.Compare(hairColour.Value, "ginger", true) != 0))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
