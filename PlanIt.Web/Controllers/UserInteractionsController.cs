using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;
using Newtonsoft.Json;
using PlanIt.Entities;
using PlanIt.Web.Models;

namespace PlanIt.Web.Controllers
{
    public class UserInteractionsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISharingService _sharingService;
        private readonly IPlanService _planService;

        public UserInteractionsController(IUserService userService, ISharingService sharingService, IPlanService planService)
        {
            _userService = userService;
            _sharingService = sharingService;
            _planService = planService;
        }

        // GET: UserInteractions
        //for perspective: view for interaction log
        public ActionResult Index()
        {
            try
            {
                List<SharedPlanUser> sharingData = _sharingService.GetIncommingSharingDataWithStatus(HttpContext.User.Identity.Name, "Pending");
                List<NotificationSummaryModel> notifications = new List<NotificationSummaryModel>();
                foreach(var data in sharingData)
                {
                    User userWhoSharedPlan = _userService.GetUserById(data.UserOwnerId);
                    Plan sharedPlan = _planService.GetPlanById(data.PlanId);

                    notifications.Add(new NotificationSummaryModel
                    {
                        SharedPlanUserId = data.Id,
                        UserWhoSharedPlan = userWhoSharedPlan,
                        SharingDateTime = data.SharingDateTime,
                        SharedPlan = sharedPlan
                    });
                }
                return View(new NotificationViewModel
                {
                    Notifications = notifications
                });
            }
            catch (Exception)
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        [HttpGet]
        public string GetUsersByPartOfEmails(string partOfEmail)
        {
            var emails = _userService.GetUsersEmailsByEmailSubstring(partOfEmail);
            return JsonConvert.SerializeObject(emails);
        }

        public ActionResult AcceptSharedPlan(int sharedPlanUserId)
        {
            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, "Accepted");
            return RedirectToAction("Index", "UserInteractions");
        }

        public ActionResult DeclineSharedPlan(int sharedPlanUserId)
        {
            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, "Declined");
            return RedirectToAction("Index", "UserInteractions");
        }

        public void CommentPlan()
        {

        }

        public void CommentPlanItem()
        {

        }

        [HttpPost]
        public JsonResult SharePlan(int planId, string toUserEmail)
        {
            string message;

            try
            {
                _sharingService.SharePlan(planId, HttpContext.User.Identity.Name, toUserEmail);
                message = "Plan was successfully shared!";
            }
            catch (Exception)
            {
                message = "Server error! Plan wasn't shared.";
            }

            return Json(new { message });
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetNumberOfIncommingPlansWithPendingStatus()
        {
            string userEmail = HttpContext.User.Identity.Name;
            int n = _sharingService.GetIncommingPlansWithStatus(userEmail, "Pending").Count;
            return Json(new { n });
        }
    }
}