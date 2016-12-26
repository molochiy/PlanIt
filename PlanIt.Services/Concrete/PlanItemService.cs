using System.Collections.Generic;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;

namespace PlanIt.Services.Concrete
{
    public class PlanItemService: BaseService, IPlanItemService
    {
        public PlanItemService(IRepository repository) : base(repository)
        {
        }

        public PlanItem GetPlanItemById(int id)
        {
            var planItem = _repository.GetSingle<PlanItem>(u => u.Id == id);

            return planItem;
        }

        public PlanItem SavePlanItem(PlanItem planItem)
        {
            return _repository.Insert<PlanItem>(planItem);
        }

        public PlanItem UpdatePlanItem(PlanItem planItem)
        {
            return _repository.Update<PlanItem>(planItem);
        }

        public ICollection<PlanItem> GetPlanItemsByPlanId(int id)
        {
            var planItems = _repository.Get<PlanItem>(u => u.PlanId == id);
            return planItems;
        }
    }
}
