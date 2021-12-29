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

namespace LatinMedia.Web.Pages.Academy.RhgAcClass
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
        public InformationUserViewModel InformationUserViewModel { get; set; }
        public List<ShowCourseForAdminViewModel> CourseList { get; set; }
        public void OnGet()
        {
            InformationUserViewModel = _userService.GetUserInformation(User.Identity.GetMobile());
            CourseList = _courseService.GetArchivedCoursesForAcUsers(InformationUserViewModel.AcademyId);
        }
    }
}