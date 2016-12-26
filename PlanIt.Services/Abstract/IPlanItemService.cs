using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface IPlanItemService
    {
        PlanItem GetPlanItemById(int id);

        PlanItem SavePlanItem(PlanItem plan);

        PlanItem UpdatePlanItem(PlanItem plan);

        ICollection<PlanItem> GetPlanItemsByPlanId(int id);
    }
}
