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
    public class PlanService : BaseService, IPlanService
    {
        public PlanService(IRepository repository) : base(repository)
        {
        }

        public Plan GetPlanById(int id)
        {
            var plan = _repository.GetSingle<Plan>(p => p.Id == id && !p.IsDeleted, p => p.PlanItems);
            
            return plan;
        }

        public IEnumerable<Plan> GetPlansByUserId(int id)
        {
            var plans = _repository.Get<Plan>(p => p.UserId == id && !p.IsDeleted, 
                p => p.PlanItems,
                p => p.Comments.Select(c => c.User),
                p => p.User);

            return plans;
        }

        public void SavePlan(Plan plan)
        {
           _repository.Insert<Plan>(plan);
        }

        public void SaveComment(Comment comment)
        {
            _repository.Insert<Comment>(comment);
        }

        public void UpdatePlan(Plan plan)
        {
            _repository.Update<Plan>(plan);
        }

        public List<Comment> GetAllCommentsByPlanId(int planId)
        {
            return _repository.Get<Comment>(c => c.PlanId == planId);
        }

        public List<Plan> GetAllPublicPlansByUserId(int userId)
        {
            int acceptedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Accepted").Id;
            List<int> sharedPlansWhereUserIsOwner = _repository.Get<SharedPlanUser>(s => s.UserOwnerId == userId && s.SharingStatusId == acceptedStatusId).Select(s => s.PlanId).ToList();
            List<int> sharedPlansWhereUserIsReceiver = _repository.Get<SharedPlanUser>(s => s.UserReceiverId == userId && s.SharingStatusId == acceptedStatusId).Select(s => s.PlanId).ToList();
            List<int> plansIds = sharedPlansWhereUserIsOwner.Union(sharedPlansWhereUserIsReceiver).ToList();
            List<Plan> plans = _repository.Get<Plan>(p => plansIds.Contains(p.Id));
            return plans;
        }

        public bool PlanIsPublic(int userId, int planId)
        {
            int acceptedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Accepted").Id;
            List<SharedPlanUser> sharedPlanUserOwner = _repository.Get<SharedPlanUser>(s => s.UserOwnerId == userId && s.PlanId == planId && s.SharingStatusId == acceptedStatusId);
            List<SharedPlanUser> sharedPlanUserReceiver = _repository.Get<SharedPlanUser>(s => s.UserReceiverId == userId && s.PlanId == planId && s.SharingStatusId == acceptedStatusId);
            if (sharedPlanUserOwner.Count > 0 || sharedPlanUserReceiver.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
