using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;

namespace LatinMedia.Web.Pages.Leon.Teachers
{
    [UserRoleChecker]
    [PermissionChecker(19)]
    public class DeleteTeacherModel : PageModel
    {
        private IUserService _userService;
        public DeleteTeacherModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationTeacherViewModel InformationTeacherViewModel { get; set; }
        public void OnGet(int id)
        {
            InformationTeacherViewModel = _userService.GetTeacherInformation(id);
            ViewData["TeacherId"] = id;
        }

        public IActionResult OnPost(int TeacherId)
        {
            //باید چک بشه آیا کلاس فعال یا تاریخ نرسیده داره این مدرس یا نه؟
            //یا اینکه بعد از حذف مدرس هر چی کلاس داره حذف بشه؟
            _userService.DeleteTeacher(TeacherId);
            return RedirectToPage("Index");
        }
    }
}