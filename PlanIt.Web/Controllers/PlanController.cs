using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Web.Models;
using System.Linq;
using PlanIt.Entities;
using System;
using System.Web.Security;
using System.Globalization;
using System.Collections.Generic;

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
                IEnumerable<Plan> plans = _planService.GetAllPlansByUserId(user.Id);
                foreach(var plan in plans)
                {
                    ICollection<Comment> comments = _planService.GetAllCommentsByPlanId(plan.Id);
                    if (comments.Count > 0)
                    {
                        foreach (Comment comment in comments)
                        {
                            comment.User = _userService.GetUserById(comment.UserId);
                        }
                    }
                    plan.Comments = comments;
                }
                return View(new PlanIndexViewModel
                {
                    Plans = plans
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
                if (postData.StartDate != "" && postData.StartDate != null) postBegin = Convert.ToDateTime(postData.StartDate);
                if (postData.EndDate != "" && postData.EndDate != null) postEnd = Convert.ToDateTime(postData.EndDate);
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
                    if (postData.PlanId != null)
                    {
                        Plan plan = _planService.GetPlanById(postData.PlanId.Value);
                        if (plan != null)
                        {
                            PlanItem planItem = new PlanItem
                            {
                                Title = postData.Title,
                                Description = postData.Description,
                                Begin = postBegin,
                                End = postEnd,
                                StatusId = 1,
                                IsDeleted = false,
                                PlanId = postData.PlanId.Value
                            };
                            plan.PlanItems.Add(planItem);
                            _planService.SavePlanItem(planItem);
                            _planService.UpdatePlan(plan);
                        }
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
                }
                return Json(Url.Action("Index", "Plan"));
            }
            catch (Exception)
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        public ActionResult RemovePlan(int planId, bool isItem = false)
        {
            if (isItem)
            {
                PlanItem planItemFromDb = _planService.GetPlanItemById(planId);
                planItemFromDb.IsDeleted = true;
                _planService.UpdatePlanItem(planItemFromDb);
            }
            else
            {
                Plan planFromDb = _planService.GetPlanById(planId);
                _planService.UpdatePlan(new Plan
                {
                    Id = planFromDb.Id,
                    Title = planFromDb.Title,
                    Description = planFromDb.Description,
                    Begin = planFromDb.Begin,
                    End = planFromDb.End,
                    StatusId = planFromDb.StatusId,
                    IsDeleted = true,
                    UserId = planFromDb.UserId
                });
            }
            return RedirectToAction("Index", "Plan");
        }
    }
}