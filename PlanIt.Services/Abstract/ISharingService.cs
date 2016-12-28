using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface ISharingService
    {
        SharedPlanUser SharePlan(int planId, string ownerEmail, string receiverEmail);

        SharedPlanUser RemoveParticipant(string ownerEmail, string participantEmail, int planId);

        List<SharedPlanUser> GetSharingInfoForNotifications(string userEmail);

        int GetNumberOfNotifications(string userEmail);

        SharedPlanUser ChangeSharingStatus(int sharedPlanUserId, string newSharingStatus);

        SharedPlanUser ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue);

        string GetSharingStatusById(int sharingStatusId);

        string GetOwnerEmailBySharingInfoId(int sharedPlanUserId);

        List<string> GetUsersEmailsWhoShouldGetComment(int planId);

        List<string> GetReceiversEmailsByPlanId(int planId);
    }
}
