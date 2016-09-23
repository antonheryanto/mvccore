using Microsoft.AspNetCore.Mvc;

namespace MvcCore {
    public class HomeController : Controller
    {
        readonly Db db;
        public HomeController(Db idb)
        {
            db = idb;
        }
        // [Authorize]
        [HttpGet("/")]
        public IActionResult Index() => View();

        [Route("test")]
        public IActionResult Test()
        {
            var m = db.Users.Get(1);
            return new OkObjectResult(m);
        }
    }
}
