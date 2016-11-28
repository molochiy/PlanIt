using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Web.Models;
using System.Linq;
using PlanIt.Entities;

namespace PlanIt.Web.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public ActionResult Index()
        {
            int UserID = 1;
            return View(new PlanIndexViewModel
            {
                Plans = _planService.GetAllPlansByUserId(UserID)
            });
        }
    }
}