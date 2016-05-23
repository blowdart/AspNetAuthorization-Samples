using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
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
                .SetBasePath(env.ContentRootPath)
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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie authentication middleware.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookie",
                LoginPath = new PathString("/Account/Unauthorized/"),
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
