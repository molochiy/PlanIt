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
        private readonly INotificationHub _notificationHub;

        public UserInteractionsController(IUserService userService, 
                                          ISharingService sharingService,
                                          INotificationHub notificationHub)
        {
            _userService = userService;
            _sharingService = sharingService;
            _notificationHub = notificationHub;
        }

        // GET: UserInteractions
        //for perspective: view for interaction log
        //now: only view notifications and react on them
        public ActionResult Index()
        {
            try
            {
                List<SharedPlanUser> sharingData = _sharingService.GetSharingInfoForNotifications(HttpContext.User.Identity.Name);
                List<NotificationSummaryModel> notifications = new List<NotificationSummaryModel>();

                foreach (var data in sharingData)
                {                    
                    notifications.Add(data);
                }

                return View(new NotificationViewModel
                {
                    Notifications = notifications
                });
            }
            catch (Exception exception)
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

        [HttpGet]
        public string GetReceiversEmailsOfCurrentPlan(int planId)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            var emails = _sharingService.GetReceiversEmailsByPlanId(planId);
            return JsonConvert.SerializeObject(emails);
        }

        //When you share plan with someone sharing info (SharedPlanUser) status become Pending
        //Receiver can change this status only to Accepted or Declined
        //Meening this mathod is for changing Pending status to Accepted or Declined
        public ActionResult ChangeSharedPlanUserStatus(int sharedPlanUserId, string newStatus)
        {
            _sharingService.ChangeSharingStatus(sharedPlanUserId, newStatus);
            _sharingService.ChangeOwnerWasNotifiedProperty(sharedPlanUserId, false);
            string userEmailForNotification = _sharingService.GetOwnerEmailBySharingInfoId(sharedPlanUserId);
            _notificationHub.UpdateNotification(new List<string> {userEmailForNotification});

            return RedirectToAction("Index", "UserInteractions");
        }

        public ActionResult ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue)
        {
            _sharingService.ChangeOwnerWasNotifiedProperty(sharedPlanUserId, newValue);
            return RedirectToAction("Index", "UserInteractions");
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

        [HttpPost]
        public JsonResult RemoveParticipant(string participantEmail, int planId)
        {
            var currentUserEmail = HttpContext.User.Identity.Name;
            try
            {
                _sharingService.RemoveParticipant(currentUserEmail, participantEmail, planId);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Server error! Participant wasn't deleted."
                });
            }
            return Json(new { success = true, message = "Participant was deleted!" });
        }

        [HttpGet]
        public JsonResult GetNumberOfNotifications()
        {
            string userEmail = HttpContext.User.Identity.Name;

            int numberOfNotifications = _sharingService.GetNumberOfNotifications(userEmail);

            return Json(new { numberOfNotifications }, JsonRequestBehavior.AllowGet);
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
    }
}