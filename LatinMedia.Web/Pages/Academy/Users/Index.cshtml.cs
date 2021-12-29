using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Academy.Users
{
    //[PermissionChecker(2)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public TestUsersForAdminViewModel UsersForAdminViewModel { get; set; }
        public void OnGet(int pageId=1,int take=50,string filterByLastName="",string filterByMobile="")
        {
            string ac = User.Identity.GetAcademyId();
            Int32 AcId = Int32.Parse(ac);
            ViewData["AcademyId"] = AcId;

            if (pageId > 1)
            {
                ViewData["Take"] = (pageId - 1) * take + 1;
            }
            else
            {
                ViewData["Take"] =  take;
            }

            ViewData["FilterLastName"] = filterByLastName;
            ViewData["FilterMobile"] = filterByMobile;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            UsersForAdminViewModel = _userService.GetUsers(AcId, pageId, take, filterByLastName, filterByMobile);
         
        }
    }
}