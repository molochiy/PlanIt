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

        public User UserWhoSharedPlan { get; set; }

        public DateTime SharingDateTime { get; set; }

        public Plan SharedPlan { get; set; }
    }
}