using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanIt.Entities;

namespace PlanIt.Web.Models
{
    public class NotificationSummaryModel
    {
        public int SharedPlanUserId { get; set; }

        public string SharingStatus { get; set; }

        public User UserOwner { get; set; }

        public User UserReciever { get; set; }

        public DateTime SharingDateTime { get; set; }

        public Plan SharedPlan { get; set; }

        public static implicit operator NotificationSummaryModel(SharedPlanUser sharedPlanUser)
        {
            var notificationSummaryModel = new NotificationSummaryModel
            {
                SharedPlanUserId = sharedPlanUser.Id,
                SharingStatus = sharedPlanUser.SharingStatus.Name,
                UserOwner = sharedPlanUser.UserOwner,
                UserReciever = sharedPlanUser.UserReceiver,
                SharingDateTime = sharedPlanUser.SharingDateTime,
                SharedPlan = sharedPlanUser.Plan
            };

            return notificationSummaryModel;
        }
    }
}