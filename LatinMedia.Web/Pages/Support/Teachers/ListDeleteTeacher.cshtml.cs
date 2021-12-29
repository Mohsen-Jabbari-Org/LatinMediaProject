using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;

namespace LatinMedia.Web.Pages.Support.Teachers
{
    public class ListDeleteTeacherModel : PageModel
    {
        private IUserService _userService;
        public ListDeleteTeacherModel(IUserService userService)
        {
            _userService = userService;
        }

        public TeachersForAdminViewModel TeachersForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 5, string filterByLastName = "", string filterByMobile = "")
        {
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);

            if (Request.Query.ContainsKey("fl"))
            {
                string s = Request.Query["fl"];
                if (s != "")
                    filterByLastName = Request.Query["fl"];
            }

            if (Request.Query.ContainsKey("fm"))
            {
                string s = Request.Query["fm"];
                if (s != "")
                    filterByMobile = Request.Query["fm"];
            }

            ViewData["FilterLastName"] = filterByLastName;
            ViewData["FilterMobile"] = filterByMobile;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            TeachersForAdminViewModel = _userService.GetProvinceDeleteTeacher(stateId, pageId, take, filterByLastName, filterByMobile);

            if (pageId > 1 && pageId != TeachersForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + take;
            }
            else if (pageId == TeachersForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + (TeachersForAdminViewModel.TeacherCounts % take);
            }
            else
            {
                ViewData["Take"] = take;
            }
        }
    }
}