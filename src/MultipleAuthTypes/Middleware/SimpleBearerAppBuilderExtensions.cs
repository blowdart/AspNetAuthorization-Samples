using System;
using Microsoft.AspNet.Builder;

namespace MultipleAuthTypes.Middleware
{
    public static class SimpleBearerAppBuilderExtensions
    {
        public static IApplicationBuilder UseSimpleBearerAuthentication
            (
                this IApplicationBuilder app,
                SimpleBearerOptions options
            )
        {
            return app.UseMiddleware<SimpleBearerMiddleware>(options);
        }

        public static IApplicationBuilder UseSimpleBearerAuthentication(this IApplicationBuilder app, Action<SimpleBearerOptions> configureOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new SimpleBearerOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }
            return app.UseSimpleBearerAuthentication(options);
        }
    }
}