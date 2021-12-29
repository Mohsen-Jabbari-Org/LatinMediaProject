using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.RhgClass
{
    public class ClassMoviesModel : PageModel
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public ClassMoviesModel(IUserService userService, ICourseService courseService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public List<BBBServers> BBBServers { get; set; }
        public Course Course { get; set; }
        public void OnGet(int id)
        {
            BBBServers = _courseService.GetListOfMoviesClass(id);
            Course = _courseService.GetCourseById(id);
            ViewData["ClassId"] = Course.DemoFileName;
        }
    }
}