using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Academy
{
    [UserRoleChecker]
    public class ChangePasswordModel : PageModel
    {
        private IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            ChangePasswordViewModel.OldPassword = FixedText.FixedTxt(ChangePasswordViewModel.OldPassword);
            ChangePasswordViewModel.Password = FixedText.FixedTxt(ChangePasswordViewModel.Password);
            ChangePasswordViewModel.RePassword = FixedText.FixedTxt(ChangePasswordViewModel.RePassword);
            if (!_userService.CompareOldPassword(User.Identity.GetMobile(), ChangePasswordViewModel.OldPassword))
            {
                ViewData["CssClass"] = "alert alert-danger";
                ViewData["Message"] = "کلمه عبور فعلی صحیح نمی باشد";

                return Page();
            }
            _userService.ChangeUserPassword(User.Identity.GetMobile(),ChangePasswordViewModel.Password);
            ViewData["CssClass"] = "alert alert-success";
            ViewData["Message"] = "کلمه عبور با موفقیت به روز رسانی شد";

            return Page();
        }
    }
}