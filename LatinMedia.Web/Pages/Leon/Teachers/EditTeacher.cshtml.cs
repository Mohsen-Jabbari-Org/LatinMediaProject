using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Leon.Teachers
{
    [UserRoleChecker]
    [PermissionChecker(18)]
    public class EditTeacherModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;
        private IPermissionService _permissionService;

        public EditTeacherModel(IUserService userService, IPermissionService permissionService, ICourseService courseService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _courseService = courseService;
        }

        [BindProperty]
        public EditTeacherViewModel EditTeacherViewModel { get; set; }
        public IActionResult OnGet(int id)
        {
            var genders = _userService.Genders();
            ViewData["GenderList"] = new SelectList(genders, "GenderId", "GenderName");
            ViewData["Group"] = _userService.GetTeacherGroups();
            if (_userService.GetTeacherById(id) != null)
            {
                EditTeacherViewModel = _userService.GetTeacherForShowInEditMode(id);
                EditTeacherViewModel.CityName = _userService.GetTeacherPlace(EditTeacherViewModel.CityCode);
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }

        public IActionResult OnPost(List<int> selectedTypes)
        {
            if (!ModelState.IsValid)
            {
                var genders = _userService.Genders();
                ViewData["GenderList"] = new SelectList(genders, "GenderId", "GenderName");
                ViewData["Group"] = _userService.GetTeacherGroups();
                return Page();
            }

            List<CourseGroup> groups = new List<CourseGroup>();
            groups = _userService.GetTeacherGroups();
            List<int> formattedList = new List<int>();
            foreach (var item in groups)
                formattedList.Add(item.GroupId);
            var compareList = formattedList.Except(selectedTypes).ToList();


            for (int i = 0; i < compareList.Count; i++)
            {
                if (_courseService.IsTeacherInCourse(EditTeacherViewModel.TeacherId, compareList[i]))
                {
                    string AcademyName = _courseService.TeacherInCourse(EditTeacherViewModel.TeacherId, groups[i].GroupId);
                    ModelState.AddModelError("EditTeacherViewModel.FirstName", "این مدرس در آموزشگاه " + AcademyName + " دارای کلاس آنلاین می باشد و قابلیت حذف گروه مورد نظر امکان پذیر نمی باشد");
                    var genders = _userService.Genders();
                    ViewData["GenderList"] = new SelectList(genders, "GenderId", "GenderName");
                    ViewData["Group"] = _userService.GetTeacherGroups();
                    ViewData["Group"] = _userService.GetTeacherGroups();
                    EditTeacherViewModel = _userService.GetTeacherForShowInEditMode(EditTeacherViewModel.TeacherId);
                    return Page();
                }

            }

            EditTeacherViewModel.Mobile = FixedText.FixedMobile(EditTeacherViewModel.Mobile);
            EditTeacherViewModel.FirstName = FixedText.FixedTxt(EditTeacherViewModel.FirstName);
            EditTeacherViewModel.LastName = FixedText.FixedTxt(EditTeacherViewModel.LastName);
            _userService.EditTeacherFromAdmin(EditTeacherViewModel);
            _permissionService.EditCourseGroupToNewTeacher(selectedTypes, EditTeacherViewModel.TeacherId);
            return RedirectToPage("Index");
        }
    }
}