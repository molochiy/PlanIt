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
        public ActionResult AddPlan(PlanAddPlanViewModel postData)
        {
            try
            {
                User user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
                DateTime? postBegin = null;
                DateTime? postEnd = null;
                if (postData.StartDate != "" && postData.StartDate != null) postBegin = DateTime.Parse(postData.StartDate);
                if (postData.EndDate != "" && postData.EndDate != null) postEnd = DateTime.Parse(postData.EndDate);
                if (postData.Id != null)
                {
                    _planService.UpdatePlan(new Plan {
                        Id = Convert.ToInt32(postData.Id),
                        Title = postData.Title,
                        Description = postData.Description,
                        Begin = postBegin,
                        End = postEnd,
                        StatusId = 1,
                        IsDeleted = false,
                        UserId = user.Id
                    });
                }
                else
                {
                    _planService.SavePlan(new Plan
                    {
                        Title = postData.Title,
                        Description = postData.Description,
                        Begin = postBegin,
                        End = postEnd,
                        StatusId = 1,
                        IsDeleted = false,
                        UserId = user.Id
                    });
                }
                return Json(Url.Action("Index", "Plan"));
            }
            catch (Exception e)
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        public ActionResult RemovePlan(int planId)
        {
            Plan planFromDb = _planService.GetPlanById(planId);
            _planService.UpdatePlan(new Plan {
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