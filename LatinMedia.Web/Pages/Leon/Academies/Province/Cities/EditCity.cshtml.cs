using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Academies.Province.Cities
{
    [UserRoleChecker]
    //[PermissionChecker(47)]
    public class EditCityModel : PageModel
    {
        private IUserService _userService;

        public EditCityModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CityEditViewModel CityEditViewModel { get; set; }
        public void OnGet(int CityId)
        {
            CityEditViewModel = _userService.GetCityForEdit(CityId);
            ViewData["StateId"] = CityEditViewModel.StateId;
        }

        public IActionResult OnPost(int CityId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            int cId = _userService.EditCity(CityEditViewModel, CityId);
            if(cId == 0)
            {
                ViewData["StateId"] = CityEditViewModel.StateId;
                ModelState.AddModelError("CityEditViewModel.CityName", "شهری با این نام قبلا در سامانه ثبت شده است .");
                return Page();
            }
            return Redirect("/Leon/Academies/Province/Cities/" + CityEditViewModel.StateId + "/1/0");
        }
    }
}