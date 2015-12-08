using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;

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