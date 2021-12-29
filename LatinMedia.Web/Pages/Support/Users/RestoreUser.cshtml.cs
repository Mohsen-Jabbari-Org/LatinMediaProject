using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Support.Users
{
    public class RestoreUserModel : PageModel
    {
        private IUserService _userService;

        public RestoreUserModel(IUserService userService)
        {
            _userService = userService;
        }

        public InformationUserViewModel InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            InformationUserViewModel = _userService.GetDeleteUserInformation(id);
            ViewData["UserId"] = id;
        }

        public IActionResult OnPost(int UserId)
        {
            var user = _userService.GetDeleteUserById(UserId);
            _userService.RestoreUsers(UserId);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 7, DateTime.Now, remoteIpAddress.ToString(),
                "کاربر " + user.FirstName + " " + user.LastName + " به شماره همراه " + user.Mobile + " توسط " + User.Identity.Name + " بازیابی گردید");
            return RedirectToPage("ListDeleteUsers");
        }
    }
}