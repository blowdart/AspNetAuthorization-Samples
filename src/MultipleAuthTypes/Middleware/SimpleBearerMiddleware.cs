using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Authentication;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;
using Microsoft.Framework.WebEncoders;

namespace MultipleAuthTypes.Middleware
{
    public class SimpleBearerMiddleware : AuthenticationMiddleware<SimpleBearerOptions>
    {
        public SimpleBearerMiddleware(
            RequestDelegate next,
            IOptions<SimpleBearerOptions> options,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
            ConfigureOptions<SimpleBearerOptions> configureOptions = null
            )
            : base(next, options, loggerFactory, encoder, configureOptions)
        {
        }

        protected override AuthenticationHandler<SimpleBearerOptions> CreateHandler()
        { 
            return new SimpleBearerHandler(); 
        }
    }
}