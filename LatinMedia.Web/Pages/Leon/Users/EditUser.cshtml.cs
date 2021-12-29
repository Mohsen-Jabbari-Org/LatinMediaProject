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

namespace LatinMedia.Web.Pages.Leon.Users
{
    [UserRoleChecker]
    [PermissionChecker(9)]
    public class EditUserModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IUserService userService, IPermissionService permissionService, ICourseService courseService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _courseService = courseService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public IActionResult OnGet(int id)
        {
            if (_userService.GetUserById(id) != null)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
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


        public IActionResult OnPost(List<int> selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            if(_courseService.IsUserInCourse(EditUserViewModel.UserId) && EditUserViewModel.AcademyId != EditUserViewModel.oldAcademyId)
            {
                string AcademyName = _courseService.UserInAcademy(EditUserViewModel.UserId);
                ModelState.AddModelError("EditUserViewModel.FirstName", "این کاربر در آموزشگاه " + AcademyName + " در کلاس آنلاین شرکت کرده است و قابل انتقال به آموزشگاه دیگر نمی باشد .");
                ViewData["Roles"] = _permissionService.GetRoles();
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
                _permissionService.EditRolesUser(EditUserViewModel.UserId, selectedRoles);
            }
            return RedirectToPage("Index");
        }
    }
}