using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Servers
{
    [UserRoleChecker]
    [PermissionChecker(46)]
    public class CreateServerModel : PageModel
    {
        private IUserService _userService;
        public CreateServerModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public BBBServers BBBServers { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            int serverId = _userService.AddServer(BBBServers);
            return RedirectToPage("Index");
        }
    }
}