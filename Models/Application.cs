using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MvcCore
{
    public class ApplicationService
    {
        public string ConnectionString { get; set; }
        public bool IsAuthenticated { get; set; }
        public User User { get; set; }
    }

    public class ApplicationMiddleWare
    {
        private readonly RequestDelegate _next;

        public ApplicationMiddleWare (RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, Db db, ApplicationService app)
        {
            var id = 0;
            if (int.TryParse(context.User.Identity.Name, out id)) {
                app.IsAuthenticated = true;
                app.User = db.Users.Get(id);
            }
            await _next(context);
        }
    }

    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddScoped<ApplicationService>();
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApplicationMiddleWare>();
        }
    }
}