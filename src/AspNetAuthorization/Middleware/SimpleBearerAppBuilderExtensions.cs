using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace AspNetAuthorization.Middleware
{
    public static class SimpleBearerAppBuilderExtensions
    {
        public static IApplicationBuilder UseSimpleBearerAuthentication
            (
                this IApplicationBuilder app,
                Action<SimpleBearerOptions> configureOptions = null
            )
        {
            return app.UseMiddleware<SimpleBearerMiddleware>(
                new ConfigureOptions<SimpleBearerOptions>(configureOptions ?? (o => { }))
                );
        }
    }
}