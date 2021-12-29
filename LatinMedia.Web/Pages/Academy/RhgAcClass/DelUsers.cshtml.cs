using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Academy.RhgAcClass
{
    public class DelUsersModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public DelUsersModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public Course Course { get; set; }
        public UserCourse UserCourse { get; set; }
        public User Users { get; set; }

        public void OnGet(int id)
        {
            string ac = User.Identity.GetAcademyId();
            Int32 AcId = Int32.Parse(ac);

            Course = _courseService.GetCourseById(id);
            ViewData["UserIn"] = _courseService.GetUserCourse(id);

            var groups = _courseService.GetGroupsForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

            //var levels = _courseService.GetLevelsForManageCourse();
            //ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.LevelId);

            //var types = _courseService.GetCourseTypesForManageCourse();
            //ViewData["Types"] = new SelectList(types, "Value", "Text", Course.TypeId);

            var teacher = _courseService.GetTeachersFromUsersToManageCourse(AcId);
            ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

            var users = _courseService.GetUsersFromUsersToManageCourse(id, Course.CompanyId);
            ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

            var companies = _courseService.GetAcademiesForManageCourse();
            ViewData["Companies"] = new SelectList(companies, "Value", "Text", Course.CompanyId);
        }

        public IActionResult OnPost(int id, int DUser)
        {
            _courseService.RemoveUserFromClass(DUser, id);
            ViewData["UserIn"] = _courseService.GetUserCourse(id);
            return RedirectToPage("DelUsers");
        }
    }
}