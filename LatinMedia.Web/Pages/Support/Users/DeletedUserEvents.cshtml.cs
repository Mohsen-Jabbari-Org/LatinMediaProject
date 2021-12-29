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

namespace LatinMedia.Web.Pages.Support.Users
{
    public class DeletedUserEventsModel : PageModel
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public DeletedUserEventsModel(IUserService userService, ICourseService courseService)
        {
            _courseService = courseService;
            _userService = userService;
        }
        [BindProperty]
        public List<ShowUserEventsViewModel> ShowUserEventsViewModels { get; set; }
        public void OnGet(int id)
        {

            ShowUserEventsViewModels = _courseService.GetEventOfUsers(id);
        }
    }
}