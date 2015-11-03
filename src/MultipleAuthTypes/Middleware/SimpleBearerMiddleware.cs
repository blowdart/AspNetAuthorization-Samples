using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Authentication;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;
using Microsoft.Framework.WebEncoders;
using Microsoft.AspNet.DataProtection;

namespace MultipleAuthTypes.Middleware
{
    public class SimpleBearerMiddleware : AuthenticationMiddleware<SimpleBearerOptions>
    {
        public SimpleBearerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
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