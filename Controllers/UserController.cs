using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;

namespace MvcCore
{
    public class UserController : Controller
    {
        readonly ILogger log;
        readonly Db db;
        public UserController (ILoggerFactory logger, Db dapper)
        {
            log = logger.CreateLogger<UserController>();
            db = dapper;
        }

        [Authorize]
        public IActionResult Index(int page = 1) {
            var m = db.Users.Page(page);
            return View(m);
        }

        [Route("user/{id:int}")]
        public IActionResult Details(int id = 1) {
            var m = db.Users.Get(id);
            m.Title = $"Logged {User.Current()}";
            return Json(m);
        }

        public IActionResult Login() => View();

        public async Task<IActionResult> Logged(string returnUrl = "~/")
        {
            if (User.Identities.Any(id => id.IsAuthenticated)) {
                return new OkObjectResult(User.Identity.Name);
            }
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new [] { new Claim(ClaimTypes.Name, "Anton") },
                CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.Authentication.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties {
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            log.LogInformation(4, "User Logout");
            return Redirect("~/");
        }

        public IActionResult Forbidden()
        {
            return new OkObjectResult(new { message = "Access Denied" });
        }
    }
}