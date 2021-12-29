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

namespace LatinMedia.Web.Pages.Leon.RhgClass
{
    [UserRoleChecker]
    [PermissionChecker(41)]
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
        public void OnGet(int pageId = 1, int take = 20, string filterByName = "", string filterByAcademy = "", string filterByAcademyName = "")
        {
            if (Request.Query.ContainsKey("fn"))
            {
                string s = Request.Query["fn"];
                if (s != "")
                    filterByName = Request.Query["fn"];
            }

            if (Request.Query.ContainsKey("fa"))
            {
                string s = Request.Query["fa"];
                if (s != "")
                    filterByAcademy = Request.Query["fa"];
            }

            if (Request.Query.ContainsKey("fan"))
            {
                string s = Request.Query["fan"];
                if (s != "")
                    filterByAcademyName = Request.Query["fan"];
            }

            ViewData["FilterName"] = filterByName;
            ViewData["FilterAcademy"] = filterByAcademy;
            ViewData["FilterAcademyName"] = filterByAcademyName;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            CourseListForAdminViewModel = _courseService.GetFinishedCoursesForAdmin(pageId, take, filterByName, filterByAcademy);

            if (pageId > 1 && pageId != CourseListForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + take;
            }
            else if (pageId == CourseListForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + (CourseListForAdminViewModel.CourseCounts % take);
            }
            else
            {
                ViewData["Take"] = take;
            }
        }
    }
}