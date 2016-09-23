using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace MvcCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .DapperDb()
                .AddMvc()
                .MonoRazor();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory logger)
        {
            if (env.IsDevelopment()) {
                logger.AddConsole(LogLevel.Debug);
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                LoginPath = "/user/logged",
                CookieName = "Auth",
                AutomaticChallenge = true, //enable auhtorization check
                AutomaticAuthenticate = true
            });
            
            app.UseMvcWithDefaultRoute();
        }

        
    }
}
