using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface IPlanItemService
    {
        PlanItem GetPlanItemById(int id);

        void SavePlanItem(PlanItem plan);

        void UpdatePlanItem(PlanItem plan);

        ICollection<PlanItem> GetPlanItemsByPlanId(int id);
    }
}
