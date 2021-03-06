using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Academy.Users
{
    //[PermissionChecker(5)]
    public class CompleteCourseModel : PageModel
    {
        private IUserService _userService;

        public CompleteCourseModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationUserViewModel InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            InformationUserViewModel = _userService.GetUserInformation(id);
            ViewData["UserId"] = id;
        }

        public IActionResult OnPost(int UserId)
        {
            _userService.RestoreUsers(UserId);
            return RedirectToPage("Index");
        }
    }
}