using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetAuthorization
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RolesAreOldSchoolDontDoThis", policy => policy.RequireRole("Administrator"));

                options.AddPolicy("RequireBobTheBuilder", policy => policy.RequireClaim("CanWeFixIt"));

                options.AddPolicy("Over18", policy => policy.Requirements.Add(new Authorization.Over18Requirement()));

                options.AddPolicy("Over21", policy => policy.Requirements.Add(new Authorization.MinimumAgeRequirement(21)));

                options.AddPolicy("TacoTuesday", policy => policy.Requirements.Add(new Authorization.DayRequirement(DayOfWeek.Tuesday)));

                options.AddPolicy("TequillaTacoTuesday", policy =>
                {
                    policy.Requirements.Add(new Authorization.DayRequirement(DayOfWeek.Thursday));
                    policy.Requirements.Add(new Authorization.MinimumAgeRequirement(21));
                });

                options.AddPolicy("Documents", policy => policy.RequireClaim("Documents"));

                options.AddPolicy("WebApi", policy =>
                {
                    policy.AuthenticationSchemes.Add("Bearer");
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("CookieBearer", policy =>
                {
                    policy.AuthenticationSchemes.Add("Bearer");
                    policy.AuthenticationSchemes.Add("Cookie");
                    policy.RequireAuthenticatedUser();
                });


                options.AddPolicy("NoGingers", policy =>
                { 
                    policy.Requirements.Add(new Authorization.NoGingersRequirement());
                });

                options.AddPolicy("CanEnterContosoBuilding", policy =>
                {
                    policy.Requirements.Add(new Authorization.EnterBuildingRequirement());
                });

                
            });

            services.AddSingleton<IAuthorizationHandler, Authorization.DocumentAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, Authorization.BaldAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, Authorization.NoGingersAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, Authorization.BuildingEntryAsEmployeeHandler>();
            services.AddSingleton<IAuthorizationHandler, Authorization.BuildingEntryAsVisitor>();

            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();

            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            // Add the platform handler to the request pipeline.
            app.UseIISPlatformHandler();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie authentication middleware.
            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "Cookie";
                options.LoginPath = new PathString("/Account/Unauthorized/");
                options.AccessDeniedPath = new PathString("/Account/Forbidden/");
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
