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

namespace LatinMedia.Web.Pages.Academy.Users
{
    //[PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserService _userService;

        public EditUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public IActionResult OnGet(int id)
        {
            if (_userService.GetUserById(id) != null)
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(id);
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
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                ViewData["GenderList"] = genders;
                return Page();
            }
            EditUserViewModel.FirstName = FixedText.FixedTxt(EditUserViewModel.FirstName);
            EditUserViewModel.LastName = FixedText.FixedTxt(EditUserViewModel.LastName);
            if(EditUserViewModel.Mobile != null)
                EditUserViewModel.Mobile = FixedText.FixedMobile(EditUserViewModel.Mobile);
            if (EditUserViewModel.Password != null)
                EditUserViewModel.Password = FixedText.FixedTxt(EditUserViewModel.Password);
            _userService.EditUserFromAdmin(EditUserViewModel);
            return RedirectToPage("Index");
        }
    }
}