using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Web.Models;
using System.Linq;
using PlanIt.Entities;
using System;

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

        public ActionResult AddPlan()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult AddPlan(PlanAddPlanViewModel postData)
        {
            _planService.SavePlan(new Plan {
                Title = postData.Title,
                Description = postData.Description,
                Begin = postData.StartDate,
                End = postData.EndDate,
                StatusId = 1,
                IsDeleted = false,
                UserId = 1             
            });

            return RedirectToAction("Index");
        }
    }
}