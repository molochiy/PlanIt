using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface ISharingService
    {
        void SharePlan(int planId, string fromUserEmail, string toUserEmail);

        List<SharedPlanUser> GetSharingInfoForNotifications(string userEmail);

        int GetNumberOfNotifications(string userEmail);

        void ChangeSharingStatus(int sharedPlanUserId, string newSharingStatus);

        void ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue);

        string GetSharingStatusById(int sharingStatusId);

        List<string> GetUsersEmailsForNotification(int sharedPlanUserId, string newStatus);

        List<string> GetUsersEmailsWhoShouldGetComment(int planId);
    }
}
