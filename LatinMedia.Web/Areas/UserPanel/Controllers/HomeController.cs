using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Senders;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Survay;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IOrderService _orderService;

        public HomeController(IUserService userService, IOrderService orderService) //تزریق وابستگی
        {
            _userService = userService;
            _orderService = orderService;
        }
      
        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.GetMobile()));
        }

        #region Edit Profile
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewBag.GenderList = genders;
            List<Academy> academy = new List<Academy>();
            academy = _userService.Academies();
            ViewBag.AcademyList = academy;
            return View(_userService.GetDataForEditProfileUser(User.Identity.GetMobile()));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
            _userService.EditProfile(User.Identity.GetMobile(), profile);

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
        #endregion

        #region Change Password
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
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

        #region Change Mobile Number
        [Route("UserPanel/ChangeMobile")]
        public IActionResult ChangeMobile()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangeMobile")]
        public IActionResult ChangeMobile(ChangeMobileViewModel m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }

            if (_userService.IsExsitMobile(m.Mobile))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده تکراری است");
                return View(m);
            }

            var user = _userService.GetUserByMobile(User.Identity.GetMobile());
            ChangeMobileViewModel viewModel = new ChangeMobileViewModel();
            Random r = new Random();
            viewModel.Mobile = m.Mobile;
            viewModel.Token = r.Next().ToString().Substring(0, 4);
            viewModel.UserId = user.UserId;

            #region Send SMS For Change Mobile Number

            //string bodyEmail = _viewRender.RenderToStringAsync("_ChangeMail", viewModel);
            //SendEmail.Send(email, "تغییر ایمیل کاربری", bodyEmail);

            //ارسال sms
            //SendSms.SendSMS(viewModel.Mobile, viewModel.Token);
            #endregion


            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectPermanentPreserveMethod("~/Account/SuccessRegister?FirstName=" + viewModel.UserId + "&LastName=" + viewModel.UserId
                + "&Mobile=" + viewModel.Mobile);
        }
        #endregion

        #region Survay
        [Route("UserPanel/Survay")]
        public IActionResult Survay()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewBag.GenderList = genders;
            List<Employment> employ = new List<Employment>();
            employ = _userService.Employments();
            ViewBag.EmpList = employ;
            List<Education> edu = new List<Education>();
            edu = _userService.Educations();
            ViewBag.EduList = edu;

            List<TwoItems> Tow = new List<TwoItems>();
            Tow = _userService.GetTwoItems();
            ViewBag.TowList = Tow;

            List<FourItems> Four = new List<FourItems>();
            Four = _userService.GetFourItems();
            ViewBag.FourList = Four;

            List<TeachersForSurvay> Teachers = new List<TeachersForSurvay>();
            Teachers = _userService.GetTeachers(_orderService.GetCoursForSurvay(User.Identity.GetMobile()));
            ViewBag.TeacherList = Teachers;
            return View(_userService.GetDataForSurvay(User.Identity.GetMobile()));
        }

        [HttpPost]
        [Route("UserPanel/Survay")]
        public IActionResult Survay(SurveyViewModel surveyViewModel)
        {
            surveyViewModel.UserId = Convert.ToInt32(User.Identity.GetID());
            List<TeachersForSurvay> Teachers = new List<TeachersForSurvay>();
            Teachers = _userService.GetTeachers(_orderService.GetCoursForSurvay(User.Identity.GetMobile()));
            surveyViewModel.teachersForSurvays = Teachers;
            if (!ModelState.IsValid)
                return View(surveyViewModel);
            
            _orderService.InsertPoll(surveyViewModel);

            //--------LogOut User-------------//

            //HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/UserPanel/MyOrders/UserCourses");
        }
        #endregion
    }
}