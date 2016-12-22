using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;
using Newtonsoft.Json;
using PlanIt.Entities;
using PlanIt.Web.Hubs;
using PlanIt.Web.Models;

namespace PlanIt.Web.Controllers
{
    public class UserInteractionsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISharingService _sharingService;
        private readonly IPlanService _planService;
        private readonly INotificationHub _notificationHub;

        public UserInteractionsController(IUserService userService, ISharingService sharingService, IPlanService planService, INotificationHub notificationHub)
        {
            _userService = userService;
            _sharingService = sharingService;
            _planService = planService;
            _notificationHub = notificationHub;
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
                    ICollection<Comment> comments = _planService.GetAllCommentsByPlanId(data.PlanId);
                    foreach(Comment comment in comments)
                    {
                        comment.User = _userService.GetUserById(comment.UserId);
                    }
                    sharedPlan.Comments = comments;
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
            var emails = _userService.GetEmailsForSharing(partOfEmail, currentUserEmail);

            return JsonConvert.SerializeObject(emails);
        }

        public ActionResult ChangeSharedPlanUserStatus(int sharedPlanUserId, string newStatus)
        {
            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, newStatus);
            var userEmailForNotification = _sharingService.GetUsersEmailsForNotification(sharedPlanUserId, newStatus);
            _notificationHub.UpdateNotification(userEmailForNotification);

            return RedirectToAction("Index", "UserInteractions");
        }

        public ActionResult ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue)
        {
            _sharingService.ChangeOwnerWasNotifiedProperty(sharedPlanUserId, newValue);
            return RedirectToAction("Index", "UserInteractions");
        }

        /*public ActionResult AcceptAndAddPlan(int sharedPlanUserId, int sharedPlanId)
        {
            string status = "Accepted";

            _sharingService.ChangeSharedPlanUserStatus(sharedPlanUserId, status);

            User user = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
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

            var userEmailForNotification = _sharingService.GetUsersEmailsForNotification(sharedPlanUserId, status);
            _notificationHub.UpdateNotification(userEmailForNotification);

            return RedirectToAction("Index", "UserInteractions");
        }*/

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
                if (!_userService.UserWithSpecificEmailExists(toUserEmail))
                {
                    return Json(new
                    {
                        success = false,
                        message = "User with enetered email not found!"
                    });
                }

                _sharingService.SharePlan(planId, currentUserEmail, toUserEmail);
                _notificationHub.UpdateNotification(new List<string> {toUserEmail});
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