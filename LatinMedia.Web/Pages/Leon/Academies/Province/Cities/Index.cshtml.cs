using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Academies.Province.Cities
{
    [UserRoleChecker]
    //[PermissionChecker(29)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public CitiesForAdminViewModel CitiesForAdminViewModel { get; set; }
        public void OnGet(int StateId, int pageId = 1, string fs = "", int take = 5, string filterByCity = "")
        {

            ViewData["FilterCity"] = filterByCity;

            if (filterByCity.Length > 0)
                pageId = 1;
            else
                ViewData["PageID"] = (pageId - 1) * take + 1;
            if (fs != "0")
            {
                filterByCity = fs;
                ViewData["FilterCity"] = filterByCity;
            }

            CitiesForAdminViewModel = _userService.GetCitiesForAdmin(StateId, pageId, take, filterByCity);
            ViewData["StateId"] = StateId;

            if (pageId > 1 && pageId != CitiesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + take;
            }
            else if (pageId == CitiesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + (CitiesForAdminViewModel.CityCounts % take);
            }
            else
            {
                ViewData["Take"] = take;
            }
        }
    }
}