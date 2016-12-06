using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;
using Newtonsoft.Json;

namespace PlanIt.Web.Controllers
{
    public class UserInteractionsController : Controller
    {
        private readonly IUserService _userService;

        public UserInteractionsController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserInteractions
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetUsersByPartOfEmails(string partOfEmail)
        {
            var emails = _userService.GetUsersEmailsByEmailSubstring(partOfEmail);
            return JsonConvert.SerializeObject(emails);
        }
    }
}