using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;

namespace PlanIt.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ViewResult SignUp()
        {
            return View();
        }

        public ViewResult SignIn()
        {
            return View();
        }

        public ViewResult EditProfile()
        {
            return View();
        }

        public ViewResult LogOut()
        {
            return View();
        }
        // GET: User
        public ActionResult Index()
        {
            var user = _userService.GetUserById(2);
            return View(user);
        }
    }
}