
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


    }
}
