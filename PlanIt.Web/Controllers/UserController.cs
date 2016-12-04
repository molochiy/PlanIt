using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanIt.Services.Abstract;
using PlanIt.Entities;
using System.Web.Security;
using System.Web.Helpers;
using System.Text;
using System.Net;

namespace PlanIt.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IProfileService _profileService;

        public UserController(IUserService userService, IProfileService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Models.SignUpDataViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (_userService.GetUserExistByEmail(user.Email) != null)
                {
                    ModelState.AddModelError("", "User with such email already registred");
                }
                else
                {
                    User newUser = new User();
                    newUser.Email = user.Email;
                    newUser.Password = sha256(user.Password);
                    newUser.IsEmailConfirmed = false;
                    Profile profile = new Profile();
                    newUser.Profile = profile;
                    _userService.AddUser(newUser);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Plan");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(Models.LogInDataViewModel user)
        {
            if (ModelState.IsValid && IsValid(user.Email, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View(user);
        }

        private bool IsValid(string email, string password)
        {
            bool IsValid = false;

            var user = _userService.GetUserExistByEmail(email);
            if (user != null && user.Password == sha256(password))
            {
                IsValid = true;
            }
            return IsValid;
        }

        private string sha256(string password)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var profileId = user.ProfileId;
                    Profile profile = _profileService.GetProfileById(profileId);
                    Models.ProfileEditProfileViewModel model = new Models.ProfileEditProfileViewModel();
                    model.FirstName = profile.FirstName;
                    model.LastName = profile.LastName;
                    model.PhoneNumber = profile.Phone;
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditProfile(Models.ProfileEditProfileViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = _userService.GetUserExistByEmail(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var profileId = user.ProfileId;
                    Profile profile = _profileService.GetProfileById(profileId);
                    profile.FirstName = model.FirstName;
                    profile.LastName = model.LastName;
                    profile.Phone = model.PhoneNumber;
                    _profileService.UpdateProfile(profile);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        // GET: User
        public ActionResult Index()
        {
            var user = _userService.GetUserById(2);
            return View(user);
        }
    }
}