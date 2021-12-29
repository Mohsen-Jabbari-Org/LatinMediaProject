using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Senders;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;

        public HomeController(IUserService userService) //تزریق وابستگی
        {
            _userService = userService;
        }
      
        public IActionResult Index()
        {
            return View(_userService.GetTeacherInformation(User.Identity.GetMobile()));
        }

        #region Edit Profile
        [Route("TeacherPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewBag.GenderList = genders;
            return View(_userService.GetDataForEditProfileTeacher(User.Identity.GetMobile()));
        }

        [HttpPost]
        [Route("TeacherPanel/EditProfile")]
        public IActionResult EditProfile(EditTeacherProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
            _userService.EditTeacherProfile(User.Identity.GetMobile(), profile);

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/LoginTeacher?EditProfile=true");
        }
        #endregion

        #region Change Password
        [Route("TeacherPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("TeacherPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string currentUser = User.Identity.GetMobile();
            if (!ModelState.IsValid)
                return View(change);

            if (!_userService.CompareTeacherOldPassword(currentUser, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(change);
            }

            _userService.ChangeTeacherPassword(currentUser, change.Password);
            ViewBag.IsSuccess = true;
            return View();
        }
        #endregion

        #region Content

        [Route("TeacherPanel/Content")]
        public IActionResult Content()
        {
            return View();
        }

        #endregion
    }
}