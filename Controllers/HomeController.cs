using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MvcCore {
    public class HomeController : Controller
    {
        readonly Db db;
        public HomeController(Db db)
        {
            this.db = db;
        }

        public IActionResult Index() => View();

		[Authorize]
		[Route("test")]
		public IActionResult Test() => Json(User.Info(db));
    }
}
