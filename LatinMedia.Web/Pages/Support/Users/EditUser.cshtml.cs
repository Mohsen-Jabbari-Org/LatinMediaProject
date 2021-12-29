using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Support.Users
{
    public class EditUserModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public EditUserModel(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public IActionResult OnGet(int id)
        {
            if (_userService.GetUserById(id) != null)
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(id);
                EditUserViewModel.oldAcademyId = EditUserViewModel.AcademyId;
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            else
            {
               return RedirectToPage("Index");
            }
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            if(_courseService.IsUserInCourse(EditUserViewModel.UserId) && EditUserViewModel.AcademyId != EditUserViewModel.oldAcademyId)
            {
                string AcademyName = _courseService.UserInAcademy(EditUserViewModel.UserId);
                ModelState.AddModelError("EditUserViewModel.FirstName", "این کاربر در آموزشگاه " + AcademyName + " در کلاس آنلاین شرکت کرده است و قابل انتقال به آموزشگاه دیگر نمی باشد .");
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                EditUserViewModel.oldAcademyId = EditUserViewModel.AcademyId;
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            else
            {
                _userService.EditUserFromAdmin(EditUserViewModel);
                var remoteIpAddress = Request.HttpContext.Connection.LocalIpAddress.ToString();
                _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 5, DateTime.Now, remoteIpAddress.ToString(),
                    "اطلاعات کاربری " + EditUserViewModel.FirstName + " " + EditUserViewModel.LastName + " توسط " + User.Identity.Name + " بروز رسانی گردید");
            }
            return RedirectToPage("Index");
        }
    }
}