using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                List<SharedPlanUser> sharingData = _sharingService.GetSharedPlanUserToShow(HttpContext.User.Identity.Name);
                List<NotificationSummaryModel> notifications = new List<NotificationSummaryModel>();
                foreach (var data in sharingData)
                {
                    User userOwner = _userService.GetUserById(data.UserOwnerId);
                    User userReciever = _userService.GetUserById(data.UserReceiverId);
                    Plan sharedPlan = _planService.GetPlanById(data.PlanId);
                    string sharingStatus = _sharingService.GetSharingStatusById(data.SharingStatusId);

                    notifications.Add(new NotificationSummaryModel
                    {
                        SharedPlanUserId = data.Id,
                        SharingStatus = sharingStatus,
                        UserOwner = userOwner,
                        UserReciever = userReciever,
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
        public string GetUsersByPartOfEmailsExceptCurrentUser(string partOfEmail)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var emails = _userService.GetUsersEmailsByEmailSubstringExceptCurrentUser(partOfEmail, currentUserEmail);

            return JsonConvert.SerializeObject(emails);
        }

        public ActionResult ChangeSharedPlanUserStatus(int sharedPlanUserId, string newStatus)
        {
            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, newStatus);
            return RedirectToAction("Index", "UserInteractions");
        }

        public ActionResult AcceptAndAddPlan(int sharedPlanUserId, int sharedPlanId)
        {
            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, "Accepted");
            User user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
            Plan sharedPlan = _planService.GetPlanById(sharedPlanId);
            _planService.SavePlan(new Plan
            {
                Title = sharedPlan.Title,
                Description = sharedPlan.Description,
                Begin = sharedPlan.Begin,
                End = sharedPlan.End,
                StatusId = 1,
                IsDeleted = false,
                UserId = user.Id
            });
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
            var currentUserEmail = HttpContext.User.Identity.Name;

            if (currentUserEmail == toUserEmail)
            {
                return Json(new
                {
                    success = false,
                    message = "You can't share plan with yourself."
                });
            }

            try
            {
                if (!_userService.UserExistsByEmail(toUserEmail))
                {
                    return Json(new
                    {
                        success = false,
                        message = "User with enetered email not found!"
                    });
                }

                _sharingService.SharePlan(planId, currentUserEmail, toUserEmail);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Server error! Plan wasn't shared."
                });
            }

            return Json(new { success = true, message = "Plan was successfully shared!" });
        }

        [HttpGet]
        public JsonResult GetNumberOfNotifications()
        {
            string userEmail = HttpContext.User.Identity.Name;

            int numberOfNotification = _sharingService.GetNumberOfNotificationForUser(userEmail);

            return Json(new { numberOfNotification }, JsonRequestBehavior.AllowGet);
        }
    }
}