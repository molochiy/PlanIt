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
            var plan = _repository.GetSingle<Plan>(u => u.Id == id);
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

        public void DeletePlan(Plan plan)
        {
            _repository.Update<Plan>(plan);
        }
    }
}
