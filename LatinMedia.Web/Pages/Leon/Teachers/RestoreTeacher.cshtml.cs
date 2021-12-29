using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Teachers
{
    [UserRoleChecker]
    [PermissionChecker(12)]
    public class RestoreTeacherModel : PageModel
    {
        private IUserService _userService;

        public RestoreTeacherModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationTeacherViewModel InformationTeacherViewModel { get; set; }
        public void OnGet(int id)
        {
            InformationTeacherViewModel = _userService.GetDeleteTeacherInformation(id);
            ViewData["TeacherId"] = id;
        }

        public IActionResult OnPost(int TeacherId)
        {
            _userService.RestoreTeachers(TeacherId);
            return RedirectToPage("ListDeleteTeacher");
        }
    }
}