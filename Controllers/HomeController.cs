using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MvcCore {
    public class HomeController : Controller
    {
        private readonly Db _db;
        private readonly ApplicationService _app;
        public HomeController(Db idb, ApplicationService app)
        {
            _db = idb;
            _app = app;
        }

        public IActionResult Index() => View();

        [Authorize]
        [Route("test")]
        public IActionResult Test()
        {
            return View("~/Views/User/Details.cshtml", _app.User);
        }
    }
}
