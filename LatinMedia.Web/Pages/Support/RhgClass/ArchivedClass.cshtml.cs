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
    public class ArchivedClassModel : PageModel
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public ArchivedClassModel(IUserService userService, ICourseService courseService)
        {
            _courseService = courseService;
            _userService = userService;
        }
        public List<ShowCourseForAdminViewModel> CourseList { get; set; }
        public CourseListForAdminViewModel CourseListForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 50, string filterByName = "", string filterByCity = "")
        {
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);
            ViewData["StateName"] = _userService.GetStateNameById(stateId);
            if (pageId > 1)
            {
                ViewData["Take"] = (pageId - 1) * take + 1;
            }
            else
            {
                ViewData["Take"] = take;
            }

            ViewData["FilterName"] = filterByName;
            ViewData["FilterCity"] = filterByCity;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            CourseListForAdminViewModel = _courseService.GetFinishedCoursesForSupport(stateId, pageId, take, filterByName, filterByCity);

            //CourseList = _courseService.GetCoursesForAdmin();
        }
    }
}