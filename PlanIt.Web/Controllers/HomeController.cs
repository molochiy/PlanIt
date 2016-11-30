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
            var user = _userService.GetUserById(2);

            return View(user);
        }
    }
}