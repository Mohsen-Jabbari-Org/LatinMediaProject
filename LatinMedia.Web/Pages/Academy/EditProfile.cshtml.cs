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

namespace LatinMedia.Web.Pages.Academy
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
        public EditProfileAdminViewModel EditProfileAdminViewModel { get; set; }

       
        public void OnGet()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewData["GenderList"] = genders;

            List<LatinMedia.DataLayer.Entities.User.Academy> academies = new List<LatinMedia.DataLayer.Entities.User.Academy>();
            academies = _userService.Academies();
            ViewData["AcademyList"] = academies;

            EditProfileAdminViewModel = _userService.GetDataForEditAdminProfileUser(User.Identity.GetMobile());
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            EditProfileAdminViewModel.FirstName = FixedText.FixedTxt(EditProfileAdminViewModel.FirstName);
            EditProfileAdminViewModel.LastName = FixedText.FixedTxt(EditProfileAdminViewModel.LastName);
            _userService.EditAdminProfile(User.Identity.GetMobile(),EditProfileAdminViewModel);

            //--------LogOut User-------------//

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
    }
}