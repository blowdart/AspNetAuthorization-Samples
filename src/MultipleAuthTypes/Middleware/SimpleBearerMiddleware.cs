using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MultipleAuthTypes.Middleware
{
    public class SimpleBearerMiddleware : AuthenticationMiddleware<SimpleBearerOptions>
    {
        public SimpleBearerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            SimpleBearerOptions options)
            : base(next, options, loggerFactory, encoder)
        { 
        }

        protected override AuthenticationHandler<SimpleBearerOptions> CreateHandler()
        { 
            return new SimpleBearerHandler(); 
        }
    }
}