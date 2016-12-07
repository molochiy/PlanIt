﻿using System;
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

        List<SharedPlanUser> GetSharedPlanUserDataWithStatus(string userEmail, string status);

        void ChangeSharedPlanUserStatus(int sharedPlanUserId, string newSharingStatus);

        string GetSharingStatusById(int sharingStatusId);
    }
}
