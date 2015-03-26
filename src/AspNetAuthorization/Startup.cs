using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace AspNetAuthorization
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddEnvironmentVariables();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // services.AddAuthorization(); // not needed?

            services.ConfigureAuthorization(options =>
            {
                options.AddPolicy("RequireBobTheBuilder", policy => policy.RequireClaim("CanWeFixIt"));

                options.AddPolicy("Over18", policy => policy.Requirements.Add(new AuthorizationPolicies.Over18Requirement()));

                options.AddPolicy("Over21", policy => policy.Requirements.Add(new AuthorizationPolicies.MinimumAgeRequirement(21)));

                options.AddPolicy("TacoTuesday", policy => policy.Requirements.Add(new AuthorizationPolicies.DayRequirement(DayOfWeek.Tuesday)));

                options.AddPolicy("TequillaTacoTuesday", policy =>
                    {
                        policy.Requirements.Add(new AuthorizationPolicies.DayRequirement(DayOfWeek.Wednesday));
                        policy.Requirements.Add(new AuthorizationPolicies.MinimumAgeRequirement(21));
                    });
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.
            // Add the console logger.
            loggerfactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie authentication middleware.
            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "Cookie";
                options.LoginPath = new PathString("/Home/PickIdentity");
            });

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
