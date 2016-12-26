using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Services.Abstract
{
    public interface IPlanService
    {
        Plan GetPlanById(int id);

        void SavePlan(Plan plan);

        void SaveComment(Comment comment);

        void UpdatePlan(Plan plan);

        IEnumerable<Plan> GetPlansByUserId(int id);

        List<Comment> GetAllCommentsByPlanId(int planId);

        List<Plan> GetAllPublicPlansByUserId(int userId);

        bool PlanIsPublic(int userId, int planId);
    }
}
