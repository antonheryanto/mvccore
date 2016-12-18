using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MvcCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json")
                .Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services
                .AddAuthentication(options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddDb(Configuration)
                .AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                LoginPath = "/user/login",
                CookieName = "Auth",
                AutomaticChallenge = true, //enable auhtorization check
                AutomaticAuthenticate = true
            });

            app.UseMvcWithDefaultRoute();
        }

    }
}
