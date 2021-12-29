using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace LatinMedia.Web.Pages.Support.Teachers
{
    public class CreateTeacherModel : PageModel
    {
        private IUserService _userService;
        private LatinMediaContext _context;
        private IPermissionService _permissionService;

        public CreateTeacherModel(IUserService userService, LatinMediaContext context, IPermissionService permissionService)
        {
            _userService = userService;
            _context = context;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateTeacherViewModel CreateTeacherViewModel { get; set; }
        public void OnGet()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewData["GenderList"] = genders;
            ViewData["Group"] = _userService.GetTeacherGroups();
        }

        public IActionResult OnPost(List<int> selectedTypes)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_userService.IsExsitTeacherMobile(CreateTeacherViewModel.Mobile))
            {
                ModelState.AddModelError("CreateTeacherViewModel.Mobile", "شماره موبایل وارد شده تکراری می باشد");
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                ViewData["Group"] = _permissionService.GetCourseGroup();
                return Page();
            }

            int TeacherId = _userService.AddTeacherFromAdmin(CreateTeacherViewModel);
            _permissionService.AddCourseGroupToNewTeacher(selectedTypes, TeacherId);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 4, DateTime.Now, remoteIpAddress.ToString(),
                " مدرس " + CreateTeacherViewModel.FirstName + " " + CreateTeacherViewModel.LastName + " با شماره همراه " + CreateTeacherViewModel.Mobile +
                " توسط " + User.Identity.Name + " در سامانه ثبت گردید");
            return RedirectToPage("Index");
        }
    }
}