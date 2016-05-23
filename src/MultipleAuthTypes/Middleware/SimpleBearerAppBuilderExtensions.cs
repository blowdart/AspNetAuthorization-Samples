using System;
using MultipleAuthTypes.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class SimpleBearerAppBuilderExtensions
    {

        public static IApplicationBuilder UseSimpleBearerAuthentication(
            this IApplicationBuilder app, 
            SimpleBearerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<SimpleBearerMiddleware>(options);
        }
    }
}