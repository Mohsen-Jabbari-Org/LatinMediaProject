using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Leon.Academies
{
    [UserRoleChecker]
    [PermissionChecker(32)]
    public class AddTeachersModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public AddTeachersModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public TeacherAcademy TeacherAcademy { get; set; }
        public NewTeacher NewTeacher { get; set; }

        public LatinMedia.DataLayer.Entities.User.Academy Academy { get; set; }
        public void OnGet(int AcademyId)
        {
            Academy = _userService.GetAcademy(AcademyId);
            string[] AcademyCiy = Academy.AcademyFullName.Split("/");
            Academy.AcademyFullName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();

            var users = _courseService.GetTeachersFromCityToAddAcademy(Academy.CityId, AcademyId);
            ViewData["TeacherAcademy"] = new SelectList(users, "Value", "Text");

            var companies = _courseService.GetAcademiesForManageCourse();
            ViewData["Academy"] = new SelectList(companies, "Value", "Text", AcademyId);

            ViewData["TeacherIn"] = _courseService.GetAcademiesTeachers(AcademyId);
        }

        public IActionResult OnPost(int AcademyId, int Teachers)
        {
            if (Teachers == 0)
                return RedirectToPage("AddTeachers");
            _courseService.AddTeacherInAcademy(Teachers, AcademyId);
            ViewData["TeacherIn"] = _courseService.GetAcademiesTeachers(AcademyId);
            return RedirectToPage("AddTeachers");
        }
    }
}