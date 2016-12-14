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

        void SaveComment(Comment comment);

        void SavePlanItem(PlanItem plan);

        void UpdatePlan(Plan plan);

        void UpdatePlanItem(PlanItem plan);

        IEnumerable<Plan> GetAllPlansByUserId(int id);

        ICollection<PlanItem> GetAllPlanItemsByPlanId(int id);

        List<Comment> GetAllCommentsByPlanId(int planId);

        List<Plan> GetAllPublicPlansByUserId(int userId);

        bool PlanIsPublic(int userId, int planId);
    }
}
