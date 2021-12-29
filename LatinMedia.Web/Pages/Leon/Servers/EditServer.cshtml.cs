using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Servers
{
    [UserRoleChecker]
    [PermissionChecker(47)]
    public class EditServerModel : PageModel
    {
        private IUserService _userService;

        public EditServerModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public BBBServerEditViewModel BBBServerEditViewModel { get; set; }
        public void OnGet(int id)
        {
            BBBServerEditViewModel = _userService.GetServerById(id);
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _userService.UpdateBBBServer(BBBServerEditViewModel , id);
            return RedirectToPage("Index");
        }
    }
}