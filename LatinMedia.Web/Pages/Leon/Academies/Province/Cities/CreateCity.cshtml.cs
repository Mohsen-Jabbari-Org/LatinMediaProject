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
    //[PermissionChecker(46)]
    public class CreateCityModel : PageModel
    {
        private IUserService _userService;
        public CreateCityModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateCityViewModel CreateCityViewModel { get; set; }
        public void OnGet(int StateId)
        {
            ViewData["StateId"] = StateId;
        }

        public IActionResult OnPost(int StateId)
        {
            CreateCityViewModel.StateId = StateId;
            if (!ModelState.IsValid)
                return Page();
            //CreateCityViewModel.StateId = Convert.ToInt32(ViewData["StateId"]);
            int CityId = _userService.AddCity(CreateCityViewModel);
            if (CityId == 0)
            {
                ViewData["StateId"] = CreateCityViewModel.StateId;
                ModelState.AddModelError("CreateCityViewModel.CityName", "شهری با این نام قبلا در سامانه ثبت شده است .");
                return Page();
            }
            return Redirect("/Leon/Academies/Province/Cities/" + CreateCityViewModel.StateId + "/1/0");
        }
    }
}