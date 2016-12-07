using System;
using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Web.Models
{
    public class NotificationViewModel
    {
        public IEnumerable<NotificationSummaryModel> Notifications { get; set; }
    }
}