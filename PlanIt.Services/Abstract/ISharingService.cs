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

        List<int> GetOutcommingPlans(string userEmail);
        List<int> GetOutcommingPlansWithStatus(string userEmail, string status);
        List<int> GetIncommingPlans(string userEmail);
        List<int> GetIncommingPlansWithStatus(string userEmail, string status);

        List<SharedPlanUser> GetIncomingSharingData(string userEmail);

        List<SharedPlanUser> GetIncommingSharingDataWithStatus(string userEmail, string status);

        void ChangeSharedPlanUserStatus(int sharedPlanUserId, string newSharingStatus);
    }
}
