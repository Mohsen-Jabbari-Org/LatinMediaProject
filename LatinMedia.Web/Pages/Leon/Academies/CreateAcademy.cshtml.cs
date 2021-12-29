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

namespace LatinMedia.Web.Pages.Leon.Academies
{
    [UserRoleChecker]
    [PermissionChecker(30)]
    public class CreateAcademyModel : PageModel
    {
        private IUserService _userService;
        public CreateAcademyModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateAcademyViewModel CreateAcademyViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Type"] = _userService.GetAcademyTypes();
            ViewData["Valid"] = _userService.GetAcademyValidTimes();
            List<BBBServers> servers = new List<BBBServers>();
            servers = _userService.GetServers();
            ViewData["ServerList"] = servers;
        }

        public IActionResult OnPost(List<int> selectedTypes, List<int> selectedTimes)
        {
            CreateAcademyViewModel.AcademyFullName += " / " + CreateAcademyViewModel.AcademyName;
            CreateAcademyViewModel.CreateDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["Type"] = _userService.GetAcademyTypes();
                ViewData["Valid"] = _userService.GetAcademyValidTimes();
                return Page();
            }

            if (_userService.IsExsitAcademy(CreateAcademyViewModel.CityId , CreateAcademyViewModel.AcademyFullName))
            {
                ModelState.AddModelError("CreateAcademyViewModel.AcademyName", "آموزشگاه وارد شده وارد شده تکراری می باشد");
                ViewData["Type"] = _userService.GetAcademyTypes();
                ViewData["Valid"] = _userService.GetAcademyValidTimes();
                List<BBBServers> servers = new List<BBBServers>();
                servers = _userService.GetServers();
                ViewData["ServerList"] = servers;
                return Page();
            }

            int AcademyId = _userService.AddAcademyFromAdmin(CreateAcademyViewModel);
           _userService.AddTypesToAcademy(selectedTypes, AcademyId);
            _userService.AddValidTimesToAcademy(selectedTimes, AcademyId);
            return RedirectToPage("Index");
        }
    }
}