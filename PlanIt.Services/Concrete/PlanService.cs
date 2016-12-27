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

        public List<PlanItem> FilterPlanItems(ICollection<PlanItem> planItems)
        {
            List<PlanItem> filtered = new List<PlanItem>();
            if(planItems.Count > 0)
            {
                foreach(var item in planItems)
                {
                    if(!item.IsDeleted)
                    {
                        filtered.Add(item);
                    }
                }
            }
            return filtered;
        }

        public Plan GetPlanById(int id)
        {
            var plan = _repository.GetSingle<Plan>(p => p.Id == id && !p.IsDeleted,
                p => FilterPlanItems(p.PlanItems),
                p => p.Comments.Select(c => c.User),
                p => p.User);

            return plan;
        }

        public IEnumerable<Plan> GetPlansByUserId(int id)
        {
            var plans = _repository.Get<Plan>(p => p.UserId == id && !p.IsDeleted,
                p => FilterPlanItems(p.PlanItems),
                p => p.Comments.Select(c => c.User),
                p => p.User);

            return plans;
        }

        public Plan SavePlan(Plan plan)
        {
           return _repository.Insert<Plan>(plan);
        }

        public Comment SaveComment(Comment comment)
        {
            return _repository.Insert<Comment>(comment);
        }

        public Plan UpdatePlan(Plan plan)
        {
            return _repository.Update<Plan>(plan);
        }

        public List<Plan> GetAllPublicPlansByUserId(int userId)
        {
            int acceptedStatusId = _repository.GetSingle<SharingStatus>(s => s.Name == "Accepted").Id;

            List<int> sharedPlansWhereUserIsOwner = _repository.Get<SharedPlanUser>(s => s.UserOwnerId == userId && s.SharingStatusId == acceptedStatusId).Select(s => s.PlanId).ToList();
            List<int> sharedPlansWhereUserIsReceiver = _repository.Get<SharedPlanUser>(s => s.UserReceiverId == userId && s.SharingStatusId == acceptedStatusId).Select(s => s.PlanId).ToList();
            List<int> plansIds = sharedPlansWhereUserIsOwner.Union(sharedPlansWhereUserIsReceiver).ToList();
            List<Plan> plans = _repository.Get<Plan>(p => plansIds.Contains(p.Id),
                                                     p => FilterPlanItems(p.PlanItems),
                                                     p => p.Comments.Select(c => c.User),
                                                     p => p.User);
            return plans;
        }
    }
}
