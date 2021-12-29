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

namespace LatinMedia.Web.Areas.InspectorPanel.Controllers
{
    [Area("InspectorPanel")]
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
            return View(_userService.GetInspectorInformation(User.Identity.GetMobile()));
        }

        #region Edit Profile
        [Route("InspectorPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewBag.GenderList = genders;
            return View(_userService.GetDataForEditProfileInspector(User.Identity.GetMobile()));
        }

        [HttpPost]
        [Route("InspectorPanel/EditProfile")]
        public IActionResult EditProfile(EditInspectorProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
            _userService.EditInspectorProfile(User.Identity.GetMobile(), profile);

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
        #endregion

        #region Change Password
        [Route("InspectorPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("InspectorPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string currentUser = User.Identity.GetMobile();
            if (!ModelState.IsValid)
                return View(change);

            if (!_userService.CompareOldPassword(currentUser, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(change);
            }

            _userService.ChangeUserPassword(currentUser, change.Password);
            ViewBag.IsSuccess = true;
            return View();
        }
        #endregion
    }
}