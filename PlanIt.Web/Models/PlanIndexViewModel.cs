using System.Collections.Generic;
using PlanIt.Entities;

namespace PlanIt.Web.Models
{
    public class PlanIndexViewModel
    {
        public IEnumerable<Plan> Plans { get; set; }
    }
}