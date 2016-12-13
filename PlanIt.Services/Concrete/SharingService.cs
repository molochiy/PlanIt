
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;

namespace PlanIt.Services.Concrete
{
    public class SharingService : BaseService, ISharingService
    {
        public SharingService(IRepository repository) : base(repository)
        {
        }

        public void SharePlan(int planId, string fromUserEmail, string toUserEmail)
        {
            var sharingDateTime = DateTime.Now;
            var fromUserId = _repository.GetSingle<User>(u => u.Email == fromUserEmail).Id;
            var toUserId = _repository.GetSingle<User>(u => u.Email == toUserEmail).Id;
            var sharingStatusId = _repository.GetSingle<SharingStatus>(ss => ss.Name == "Pending").Id;
            var sharedPlanUser = new SharedPlanUser
            {
                PlanId = planId,
                SharingDateTime = sharingDateTime,
                SharingStatusId = sharingStatusId,
                UserOwnerId = fromUserId,
                UserReceiverId = toUserId
            };

            _repository.Insert(sharedPlanUser);
        }

        public List<SharedPlanUser> GetSharedPlanUserData(string userEmail)
        {
            var userId = _repository.GetSingle<User>(u => u.Email == userEmail).Id;
            return _repository.Get<SharedPlanUser>(s => s.UserReceiverId == userId);
        }

        public List<SharedPlanUser> GetSharedPlanUserToShow(string userEmail)
        {
            var userId = _repository.GetSingle<User>(u => u.Email == userEmail).Id;
            var pandingStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Pending").Id;
            var acceptedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Accepted").Id;
            var declinedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Declined").Id;
            return _repository.Get<SharedPlanUser>(
               (s => 
                    (s.UserReceiverId == userId && s.SharingStatusId == pandingStatusId) ||
                    (s.UserOwnerId == userId && s.OwnerWasNotified == false &&
                                         (s.SharingStatusId == acceptedStatusId ||
                                          s.SharingStatusId == declinedStatusId))));
        }

        public int GetNumberOfNotificationForUser(string userEmail)
        {
            return GetSharedPlanUserToShow(userEmail).Count();
        }

        public void ChangeSharedPlanUserStatus(int sharedPlanUserId, string newSharingStatus)
        {
            var sharedInfo = _repository.GetSingle<SharedPlanUser>(s => s.Id == sharedPlanUserId);
            var statusId = _repository.GetSingle<SharingStatus>(s => s.Name == newSharingStatus).Id;
            sharedInfo.SharingStatusId = statusId;
            sharedInfo.SharingDateTime = DateTime.Now;
            _repository.Update<SharedPlanUser>(sharedInfo);
        }

        public void ChangeOwnerWasNotifiedProperty(int sharedPlanUserId, bool newValue)
        {
            var sharedInfo = _repository.GetSingle<SharedPlanUser>(s => s.Id == sharedPlanUserId);
            sharedInfo.OwnerWasNotified = newValue;
            _repository.Update<SharedPlanUser>(sharedInfo);
        }

        public string GetSharingStatusById(int sharingStatusId)
        {
            return _repository.GetSingle<SharingStatus>(s => s.Id == sharingStatusId).Name;
        }
    }
}
