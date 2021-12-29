using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.RhgClass
{
    [UserRoleChecker]
    [PermissionChecker(10)]
    public class DeleteClassModel : PageModel
    {
        private ICourseService _courseService;

        public DeleteClassModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);
            ViewData["CourseId"] = id;
        }

        public IActionResult OnPost(int CourseId)
        {
            _courseService.DeleteCourse(CourseId);
            return RedirectToPage("Index");
        }
    }
}