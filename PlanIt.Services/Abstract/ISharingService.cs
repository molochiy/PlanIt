using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface ISharingService
    {
        SharedPlanUser SharePlan(int planId, string ownerEmail, string receiverEmail);

        List<SharedPlanUser> GetSharingInfoForNotifications(string userEmail);

        int GetNumberOfNotifications(string userEmail);

        SharedPlanUser ChangeSharingStatus(int sharedPlanUserId, string newSharingStatus);

        SharedPlanUser ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue);

        string GetSharingStatusById(int sharingStatusId);

        List<string> GetUsersEmailsForNotification(int sharedPlanUserId, string newStatus);

        List<string> GetUsersEmailsWhoShouldGetComment(int planId);
    }
}
