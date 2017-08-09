using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoListService
{
    /// <summary>
    ///  ---
    ///    services: active-directory
    ///    platforms: dotnet
    ///    author: jmprieur
    ///    ---
    ///    Calling a web API in an ASP.NET Core web application using Azure AD
    ///    This sample shows how to build an MVC web application that uses Azure AD for sign-in using the OpenID Connect protocol, 
    ///    and then calls a web API under the signed-in user's identity using tokens obtained via OAuth 2.0. 
    ///    This sample uses the OpenID Connect ASP.Net middleware and ADAL .Net running on ASP.NET Core.
    /// 
    /// </summary>


    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Add Authentication services.
            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add the console logger.
            loggerFactory.AddConsole(LogLevel.Debug);

            // Configure the app to use Jwt Bearer Authentication
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                Authority = String.Format(Configuration["AzureAd:AadInstance"], Configuration["AzureAD:Tenant"]),
                Audience = Configuration["AzureAd:Audience"],
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
