using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface IPlanService
    {
        List<PlanItem> FilterPlanItems(ICollection<PlanItem> planItems);

        Plan GetPlanById(int id);

        Plan SavePlan(Plan plan);

        Comment SaveComment(Comment comment);

        Plan UpdatePlan(Plan plan);

        IEnumerable<Plan> GetPlansByUserId(int id);

        List<Comment> GetAllCommentsByPlanId(int planId);

        List<Plan> GetAllPublicPlansByUserId(int userId);
    }
}
