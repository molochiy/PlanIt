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
using PlanIt.Web.Hubs;

namespace PlanIt.Web.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        private readonly IUserService _userService;
        private readonly ISharingService _sharingService;
        private readonly IPlanItemService _planItemService;
        private readonly INotificationHub _notificationHub;

        public PlanController(IPlanService planService, IUserService userService, ISharingService sharingService, INotificationHub notificationHub, IPlanItemService planItemService)
        {
            _planService = planService;
            _userService = userService;
            _sharingService = sharingService;
            _notificationHub = notificationHub;
            _planItemService = planItemService;
        }

        public ActionResult Index()
        {
            try
            {
                User user = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
                IEnumerable<Plan> plans = _planService.GetPlansByUserId(user.Id);

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

        public ActionResult PublicPlans()
        {
            try
            {
                User user = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
                IEnumerable<Plan> plans = _planService.GetAllPublicPlansByUserId(user.Id);
                foreach (var plan in plans)
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
                    plan.User = _userService.GetUserById(plan.UserId);
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

        [HttpPost]
        public JsonResult CommentPlan(string text, int planId)
        {
            var createdTime = DateTime.Now;
            var currentUserEmail = HttpContext.User.Identity.Name;

            try
            {
                _planService.SaveComment(new Comment
                {
                    PlanId = planId,
                    Text = text,
                    CreatedTime = DateTime.Now,
                    UserId = _userService.GetUserIdByEmail(currentUserEmail)
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Server error! Comment wasn't added."
                });
            }

            var commentData = Json(new { CreatedTime = createdTime.ToString(), PlanId = planId, Text = text, UserEmail = currentUserEmail });

            //Get receivers(user's email who should get comment)
            List<string> receivers = _sharingService.GetUsersEmailsWhoshouldGetComment(planId);

            _notificationHub.AddNewCommentToList(receivers, commentData);

            return Json(new
            {
                success = true,
                message = "Comment added."
            });
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
                User user = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
                DateTime? postBegin = null;
                DateTime? postEnd = null;
                if (!string.IsNullOrEmpty(postData.StartDate)) postBegin = DateTime.ParseExact(postData.StartDate, new [] { "MM/dd/yyyy" }, new CultureInfo("uk-UA"), DateTimeStyles.None);
                if (!string.IsNullOrEmpty(postData.EndDate)) postEnd = DateTime.ParseExact(postData.EndDate, new[] { "MM/dd/yyyy" }, new CultureInfo("uk-UA"), DateTimeStyles.None);
                if (postData.Id != null)
                {
                    _planService.UpdatePlan(new Plan
                    {
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
                            _planItemService.SavePlanItem(planItem);
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
            catch (Exception ex)
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        public ActionResult RemovePlan(int planId, bool isItem = false)
        {
            if (isItem)
            {
                PlanItem planItemFromDb = _planItemService.GetPlanItemById(planId);
                planItemFromDb.IsDeleted = true;
                _planItemService.UpdatePlanItem(planItemFromDb);
            }
            else
            {
                Plan planFromDb = _planService.GetPlanById(planId);
                planFromDb.IsDeleted = true;
                _planService.UpdatePlan(planFromDb);
            }
            return RedirectToAction("Index", "Plan");
        }
    }
}