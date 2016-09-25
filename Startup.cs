using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MvcCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
            services.AddSingleton<IConfiguration>(config);

            services
                .AddAuthentication(options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .DapperDb(new MySql.Data.MySqlClient.MySqlConnection(config.GetConnectionString("MySql")))
                .AddApplication()
                .AddMvc()
                .MonoRazor();
        }

        public void Configure(IApplicationBuilder app, 
            Db db,
            IHostingEnvironment env, 
            ILoggerFactory logger)
        {
            if (env.IsDevelopment()) {
                // logger.AddConsole(LogLevel.Debug);
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                LoginPath = "/user/login",
                CookieName = "Auth",
                AutomaticChallenge = true, //enable auhtorization check
                AutomaticAuthenticate = true
            });

            app.UseApplication();

            app.UseMvcWithDefaultRoute();
        }

        
    }
}
