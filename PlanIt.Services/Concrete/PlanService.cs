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
            var plan = _repository.GetSingle<Plan>(u => u.Id == id && !u.IsDeleted);
            var planItems = GetAllPlanItemsByPlanId(plan.Id).ToList();
            foreach(PlanItem item in planItems)
            {
                if (!item.IsDeleted)
                    plan.PlanItems.Add(item);
            }
            return plan;
        }

        public PlanItem GetPlanItemById(int id)
        {
            var planItem = _repository.GetSingle<PlanItem>(u => u.Id == id);
            return planItem;
        }

        public IEnumerable<Plan> GetAllPlansByUserId(int id)
        {
            var plans = _repository.Get<Plan>(u => u.UserId == id && !u.IsDeleted);
            plans.ForEach(plan => GetAllPlanItemsByPlanId(plan.Id).ToList().ForEach(item => { if (!item.IsDeleted) plan.PlanItems.Add(item); }));
            return plans;
        }

        public ICollection<PlanItem> GetAllPlanItemsByPlanId(int id)
        {
            var planItems = _repository.Get<PlanItem>(u => u.PlanId == id);
            return planItems;
        }

        public void SavePlan(Plan plan)
        {
           _repository.Insert<Plan>(plan);
        }

        public void SaveComment(Comment comment)
        {
            _repository.Insert<Comment>(comment);
        }


        public void UpdatePlanItem(PlanItem planItem)
        {
            _repository.Update<PlanItem>(planItem);
        }

        public void SavePlanItem(PlanItem planItem)
        {
            _repository.Insert<PlanItem>(planItem);
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
            //Plan items should upload for current plan on current request
            /*foreach(var p in plans)
            {
                ICollection<PlanItem> items = GetAllPlanItemsByPlanId(p.Id);
                p.PlanItems = items;
            }*/
            return plans;
        }

        public bool PlanIsPublic(int userId, int planId)
        {
            int acceptedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Accepted").Id;
            List<SharedPlanUser> sharedPlanUserOwner = _repository.Get<SharedPlanUser>(s => s.UserOwnerId == userId && s.PlanId == planId && s.SharingStatusId == acceptedStatusId);
            List<SharedPlanUser> sharedPlanUserReceiver = _repository.Get<SharedPlanUser>(s => s.UserReceiverId == userId && s.PlanId == planId && s.SharingStatusId == acceptedStatusId);
            if (sharedPlanUserOwner.Count > 0 || sharedPlanUserReceiver.Count > 0)
                return true;
            return false;
        }
    }
}
