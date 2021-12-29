using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.HttpOverrides;

namespace LatinMedia.Web.Pages.Support
{
    [UserRoleChecker]
    public class EditProfileModel : PageModel
    {
        private IUserService _userService;

        public EditProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public EditSupportProfileViewModel EditSupportProfileViewModel { get; set; }

       
        public void OnGet()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewData["GenderList"] = genders;

            EditSupportProfileViewModel = _userService.GetDataForEditSupportProfileUser(User.Identity.GetMobile());
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

            EditSupportProfileViewModel.FirstName = FixedText.FixedTxt(EditSupportProfileViewModel.FirstName);
            EditSupportProfileViewModel.LastName = FixedText.FixedTxt(EditSupportProfileViewModel.LastName);
            _userService.EditSupportProfile(User.Identity.GetMobile(),EditSupportProfileViewModel);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 5, DateTime.Now, remoteIpAddress.ToString(),
                "اطلاعات کاربری توسط " + User.Identity.Name + " در سامانه بروز رسانی گردید");

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
    }
}