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

        List<SharedPlanUser> GetSharedPlanUserData(string userEmail);

        List<SharedPlanUser> GetSharedPlanUserToShow(string userEmail);

        int GetNumberOfNotificationForUser(string userEmail);

        void ChangeSharedPlanUserStatus(int sharedPlanUserId, string newSharingStatus);

        void ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue);

        string GetSharingStatusById(int sharingStatusId);

        List<string> GetUsersEmailsForNotification(int sharedPlanUserId, string newStatus);

        List<string> GetUsersEmailsWhoshouldGetComment(int planId);
    }
}
