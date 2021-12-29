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

namespace LatinMedia.Web.Pages.Leon.Academies.Province
{
    public class SupportTellModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public SupportTellModel(ICourseService courseService , IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        public SupportTell SupportTell { get; set; }
        public void OnGet(int StateId)
        {
            ViewData["Tell"] = _userService.GetStatesTells(StateId);
        }

        public IActionResult OnPost(string Tell , int StateId)
        {
            if (Tell != null)
            {
                _userService.AddSupportTell(Tell, StateId);
                ViewData["Tell"] = _userService.GetStatesTells(StateId);
                return Page();
            }
            else
            {
                ViewData["Tell"] = _userService.GetStatesTells(StateId);
                return Page();
            }
        }
    }
}