using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.RhgClass
{
    [UserRoleChecker]
    [PermissionChecker(39)]
    public class ActiveClassUsersListModel : PageModel
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public ActiveClassUsersListModel(IUserService userService, ICourseService courseService)
        {
            _courseService = courseService;
            _userService = userService;
        }
        [BindProperty]
        public Course Course { get; set; }
        public UserCourse UserCourse { get; set; }
        public User Users { get; set; }
        public List<ShowUsersInClassViewModel> showUsersInClassViewModels { get; set; }
        public void OnGet(int id)
        {
            ViewData["CourseId"] = id;
            showUsersInClassViewModels = _courseService.GetUsersInClass(id);
            Course = _courseService.GetCourseById(id);
        }
    }
}