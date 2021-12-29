using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Support.RhgClass
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public IndexModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        public List<ShowCourseForAdminViewModel> CourseList { get; set; }
        public CourseListForAdminViewModel CourseListForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 50, string filterByName = "", string filterByAcademy = "", string filterByAcademyName = "")
        {
            string AcId = User.Identity.GetAcademyId();
            var stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);
            ViewData["StateName"] = _userService.GetStateNameById(stateId);
            ViewData["StateId"] = stateId;
            if (pageId > 1)
            {
                ViewData["Take"] = (pageId - 1) * take + 1;
            }
            else
            {
                ViewData["Take"] = take;
            }

            ViewData["FilterName"] = filterByName;
            ViewData["FilterAcademy"] = filterByAcademy;
            ViewData["FilterAcademyName"] = filterByAcademyName;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            CourseListForAdminViewModel = _courseService.GetCoursesForSupport(stateId, pageId, take, filterByName, filterByAcademy);

            //CourseList = _courseService.GetCoursesForAdmin();
        }
    }
}