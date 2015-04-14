using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Authentication;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;

namespace AspNetAuthorization.Middleware
{
    public class SimpleBearerMiddleware : AuthenticationMiddleware<SimpleBearerOptions>
    {
        public SimpleBearerMiddleware(
            RequestDelegate next,
            IOptions<SimpleBearerOptions> options,
            ConfigureOptions<SimpleBearerOptions> configureOptions = null)
            : base(next, options, configureOptions)
        {
        }

        protected override AuthenticationHandler<SimpleBearerOptions> CreateHandler()
        { 
            return new SimpleBearerHandler(); 
        }
    }
}