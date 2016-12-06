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
        private readonly ISharingService _sharingService;

        public UserInteractionsController(IUserService userService, ISharingService sharingService)
        {
            _userService = userService;
            _sharingService = sharingService;
        }

        // GET: UserInteractions
        //for perspective: view for interaction log
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

        public ActionResult AddPlansMember()
        {
            return RedirectToAction("Index", "Plan");
        }

        public ActionResult AddPlanItemsMember()
        {
            return RedirectToAction("Index", "Plan");
        }

        public void CommentPlan()
        {

        }

        public void CommentPlanItem()
        {

        }

        public ActionResult SharePlan(int planId, string toUserEmail)
        {
            _sharingService.SharePlan(planId, HttpContext.User.Identity.Name, toUserEmail);

            return Json(Url.Action("Index", "Plan"));
        }
    }
}