using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;

namespace LatinMedia.Web.Pages.Support.Teachers
{
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
            var teacher = _userService.GetTeacherById(TeacherId);
            _userService.DeleteTeacher(TeacherId);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 6, DateTime.Now, remoteIpAddress.ToString(),
                "مدرس" + teacher.FirstName + " " + teacher.LastName + " به شماره همراه " + teacher.Mobile + " توسط " +
                User.Identity.Name + " حذف گردید");
            return RedirectToPage("Index");
        }
    }
}