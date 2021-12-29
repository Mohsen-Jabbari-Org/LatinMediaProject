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
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;

        public DeleteUserModel(IUserService userService)
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
            var user = _userService.GetUserById(UserId);
            _userService.DeleteUser(UserId);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 6, DateTime.Now, remoteIpAddress.ToString(),
                "کاربر " + user.FirstName + " " + user.LastName + " به شماره همراه " + user.Mobile + " توسط " + User.Identity.Name + " حذف گردید");
            return RedirectToPage("Index");
        }
    }
}