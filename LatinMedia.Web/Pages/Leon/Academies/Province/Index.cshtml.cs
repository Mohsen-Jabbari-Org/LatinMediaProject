using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Academies.Province
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

        public StatesForAdminViewModel StatesForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 5, string filterByState = "")
        {
            if (Request.Query.ContainsKey("fs"))
            {
                string s = Request.Query["fs"];
                if (s != "")
                    filterByState = Request.Query["fs"];
            }

            ViewData["FilterState"] = filterByState;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            StatesForAdminViewModel = _userService.GetStatesForAdmin(pageId, take, filterByState);

            if (pageId > 1 && pageId != StatesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + take;
            }
            else if (pageId == StatesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + (StatesForAdminViewModel.StatesCounts % take);
            }
            else
            {
                ViewData["Take"] = take;
            }
        }
    }
}