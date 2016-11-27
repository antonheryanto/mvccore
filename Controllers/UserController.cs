using System;
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
        readonly Db db;
        public UserController (Db db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index(int page = 1) {
            var m = db.Users.Page(page);
            return View(m);
        }

        [Route("user/{id:int}")]
        [Authorize]
        public IActionResult Details(int id = 1) {
            var m = db.Users.Get(id);
            return View(m);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User m, string returnUrl = "~/")
        {
            if (string.IsNullOrWhiteSpace(m.Name)) ModelState.AddModelError(nameof(m.Name), "Required");
            if (string.IsNullOrWhiteSpace(m.Password)) ModelState.AddModelError(nameof(m.Password), "Required");
            if (!ModelState.IsValid) return View(m);
            
            var user = db.Query<User>("select * from users where email=@name and password=@password",
                new { m.Name, password = m.Password.Encrypt() }).FirstOrDefault();
            if (user == null) return View(m);

            var u = new ClaimsPrincipal(new ClaimsIdentity(
                new [] { new Claim(ClaimTypes.Name, user.Id.ToString()) },
                CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.Authentication.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, u, new AuthenticationProperties {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        public IActionResult Forbidden()
        {
            return new OkObjectResult(new { message = "Access Denied" });
        }
    }
}