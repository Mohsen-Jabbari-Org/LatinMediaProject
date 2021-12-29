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

namespace LatinMedia.Web.Pages.Academy.Users
{
    //[PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;

        public CreateUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            string ac = User.Identity.GetAcademyId();
            Int32 AcId = Int32.Parse(ac);
            var academy = _userService.GetAcademyName(AcId);
            ViewData["Academies"] = new SelectList(academy, "Value", "Text");

            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            ViewData["GenderList"] = genders;
        }

        public IActionResult OnPost(List<int> selectedRoles)
        {
            string ac = User.Identity.GetAcademyId();
            Int32 AcId = Int32.Parse(ac);
            CreateUserViewModel.AcademyId = AcId;
            if (!ModelState.IsValid)
            {
                var academy = _userService.GetAcademyName(AcId);
                ViewData["Academies"] = new SelectList(academy, "Value", "Text");
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            if(CreateUserViewModel.Mobile.Length < 11)
            {
                ModelState.AddModelError("CreateUserViewModel.Mobile", "تعداد ارقام شماره موبایل کمتر از 11 می باشد");
                var academy = _userService.GetAcademyName(AcId);
                ViewData["Academies"] = new SelectList(academy, "Value", "Text");
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            CreateUserViewModel.FirstName = FixedText.FixedTxt(CreateUserViewModel.FirstName);
            CreateUserViewModel.LastName = FixedText.FixedTxt(CreateUserViewModel.LastName);
            CreateUserViewModel.Mobile = FixedText.FixedMobile(CreateUserViewModel.Mobile);
            CreateUserViewModel.Password = FixedText.FixedTxt(CreateUserViewModel.Password);
            if (_userService.IsExsitMobile(CreateUserViewModel.Mobile))
            {
                var academy = _userService.GetAcademyName(AcId);
                ViewData["Academies"] = new SelectList(academy, "Value", "Text");

                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                ModelState.AddModelError("CreateUserViewModel.Mobile", "شماره موبایل وارد شده تکراری می باشد");
                return Page();
            }

            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            return RedirectToPage("Index");
        }
             

    
    }
}