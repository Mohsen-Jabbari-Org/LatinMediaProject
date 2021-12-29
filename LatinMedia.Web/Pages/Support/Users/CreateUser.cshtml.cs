using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace LatinMedia.Web.Pages.Support.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public CreateUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewData["GenderList"] = genders;
            int StateId = _userService.GetStateFoAcademy(_userService.GetCityForAcademies(Convert.ToInt32(User.Identity.GetAcademyId())));
            ViewData["StateId"] = StateId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                int StateId = _userService.GetStateFoAcademy(Convert.ToInt32(User.Identity.GetAcademyId()));
                ViewData["StateId"] = StateId;
                return Page();
            }

            if (_userService.IsExsitMobile(FixedText.FixedMobile(CreateUserViewModel.Mobile)))
            {
                ModelState.AddModelError("CreateUserViewModel.Mobile", "شماره موبایل وارد شده تکراری می باشد");
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                int StateId = _userService.GetStateFoAcademy(Convert.ToInt32(User.Identity.GetAcademyId()));
                ViewData["StateId"] = StateId;
                return Page();
            }

            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 4, DateTime.Now, remoteIpAddress.ToString(), "کاربر جدید " + CreateUserViewModel.FirstName +
                 " " + CreateUserViewModel.LastName + "با شماره همراه " + CreateUserViewModel.Mobile + " توسط " + User.Identity.Name + " ثبت گردید");
            return RedirectToPage("Index");
        }
             

    
    }
}