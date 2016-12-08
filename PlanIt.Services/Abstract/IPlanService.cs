using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface IPlanService
    {
        Plan GetPlanById(int id);

        PlanItem GetPlanItemById(int id);

        void SavePlan(Plan plan);

        void UpdatePlan(Plan plan);

        IEnumerable<Plan> GetAllPlansByUserId(int id);

        IEnumerable<PlanItem> GetAllPlanItemsByPlanId(int id);
    }
}
