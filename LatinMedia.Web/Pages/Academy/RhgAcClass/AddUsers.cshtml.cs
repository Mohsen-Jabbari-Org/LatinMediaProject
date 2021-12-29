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
    public class AddUsersModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public AddUsersModel(ICourseService courseService , IUserService userService)
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

            var teacher = _courseService.GetTeachersFromUsersToManageCourse(AcId);
            ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

            var companies = _courseService.GetAcademiesForManageCourse();
            ViewData["Companies"] = new SelectList(companies, "Value", "Text", Course.CompanyId);
        }

        public IActionResult OnPost(int id , int Users)
        {
            Course = _courseService.GetCourseById(id);
            var user = _courseService.GetUserCourse(id);

            int maxUsers = Course.MaxUsers - 2;
            if(Users == 0)
                return RedirectToPage("AddUsers");
            if(user.Count < maxUsers)
            {
                _courseService.AddUserInClass(Users, id);
                ViewData["UserIn"] = _courseService.GetUserCourse(id);
                return RedirectToPage("AddUsers");
            }
            else
            {
                string ac = User.Identity.GetAcademyId();
                Int32 AcId = Int32.Parse(ac);

                Course = _courseService.GetCourseById(id);
                ViewData["UserIn"] = _courseService.GetUserCourse(id);

                var groups = _courseService.GetGroupsForManageCourse();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

                var teacher = _courseService.GetTeachersFromUsersToManageCourse(AcId);
                ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

                var users = _courseService.GetUsersFromUsersToManageCourse(id, Course.CompanyId);
                ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

                var companies = _courseService.GetAcademiesForManageCourse();
                ViewData["Companies"] = new SelectList(companies, "Value", "Text", Course.CompanyId);
                ModelState.AddModelError("Course.CourseFaTitle", "تعداد کاربران مجاز برای این کلاس " + maxUsers + " می باشد .");
                return Page();
            }
        }
    }
}