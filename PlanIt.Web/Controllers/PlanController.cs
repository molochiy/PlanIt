using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Web.Models;
using System.Linq;
using PlanIt.Entities;
using System;
using System.Web.Security;

namespace PlanIt.Web.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IUserService _userService;

        public PlanController(IPlanService planService, IUserService userService)
        {
            _planService = planService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            try
            {
                User user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
                return View(new PlanIndexViewModel
                {
                    Plans = _planService.GetAllPlansByUserId(user.Id)
                });
            }
            catch (Exception)
            {
                return RedirectToAction("LogIn", "User");
            }
                
        }

        public ActionResult AddPlan()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult AddPlan(PlanAddPlanViewModel postData)
        {
            try
            {
                User user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
                _planService.SavePlan(new Plan
                {
                    Title = postData.Title,
                    Description = postData.Description,
                    Begin = postData.StartDate,
                    End = postData.EndDate,
                    StatusId = 1,
                    IsDeleted = false,
                    UserId = user.Id
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        public ActionResult RemovePlan(int planId)
        {
            Plan planFromDb = _planService.GetPlanById(planId);
            _planService.DeletePlan(new Plan {
                Id = planFromDb.Id,
                Title = planFromDb.Title,
                Description = planFromDb.Description,
                Begin = planFromDb.Begin,
                End = planFromDb.End,
                StatusId = planFromDb.StatusId,
                IsDeleted = true,
                UserId = planFromDb.UserId
            });
            return RedirectToAction("Index", "Plan");
        }
    }
}