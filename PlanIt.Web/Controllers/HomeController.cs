using System.Web.Mvc;
using PlanIt.Services.Abstract;

namespace PlanIt.Web.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Plan");
            }
            return View();
        }
    }
}