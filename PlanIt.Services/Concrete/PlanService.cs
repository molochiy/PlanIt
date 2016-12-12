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

        public IEnumerable<PlanItem> GetAllPlanItemsByPlanId(int id)
        {
            var planItems = _repository.Get<PlanItem>(u => u.PlanId == id);
            return planItems;
        }

        public void SavePlan(Plan plan)
        {
           _repository.Insert<Plan>(plan);
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
    }
}
