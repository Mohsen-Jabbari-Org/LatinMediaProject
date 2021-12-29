using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace LatinMedia.Web.Pages.Support.Academies
{
    public class EditAcademyModel : PageModel
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public EditAcademyModel(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        [BindProperty]
        public EditAcademyViewModel EditAcademyViewModel { get; set; }
        public void OnGet(int AcademyId)
        {
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);
            ViewData["StateId"] = stateId;
            EditAcademyViewModel = _userService.GetAcademyForEdit(AcademyId);
            ViewData["Type"] = _userService.GetAcademyTypes();
            ViewData["Valid"] = _userService.GetAcademyValidTimes();
            string[] AcademyCiy = EditAcademyViewModel.AcademyFullName.Split("/");
            EditAcademyViewModel.CityName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();
            List<BBBServers> servers = new List<BBBServers>();
            servers = _userService.GetServers();
            ViewData["ServerList"] = servers;
        }

        public IActionResult OnPost(int AcademyId, List<int> selectedTypes, List<int> selectedTimes)
        {
            EditAcademyViewModel.AcademyFullName = string.Empty;
            EditAcademyViewModel.AcademyFullName = EditAcademyViewModel.CityName + " / " + EditAcademyViewModel.AcademyName;
            EditAcademyViewModel.CreateDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewData["Type"] = _userService.GetAcademyTypes();
                ViewData["Valid"] = _userService.GetAcademyValidTimes();
                List<BBBServers> servers = new List<BBBServers>();
                servers = _userService.GetServers();
                ViewData["ServerList"] = servers;
                return Page();
            }

            List<ValidTimesCourse> Valid = new List<ValidTimesCourse>();
            Valid = _userService.GetAcademyValidTimes();
            List<int> formattedTimeList = new List<int>();
            foreach (var item in Valid)
                formattedTimeList.Add(item.ValidTimesCourseId);

            var compareList = formattedTimeList.Except(selectedTimes).ToList();

            //for (int i = 0; i < Type.Count; i++)
            //{
            //    if (Type[i].AcademyTypeId != formattedList[i])
            //    {
            //        if (_courseService.IsAcademyTypeInCourse(AcademyId, Type[i].AcademyTypeId))
            //        {
            //            ModelState.AddModelError("EditAcademyViewModel.AcademyName", "از گروه های حذف شده در قسمت کلاس ها استفاده شده است. لطفا ابتدا گروه کلاس ویرایش و سپس از طریق ادمینحذف شود.");
            //            EditAcademyViewModel = _userService.GetAcademyForEdit(AcademyId);
            //            ViewData["Type"] = _userService.GetAcademyTypes();
            //            ViewData["Valid"] = _userService.GetAcademyValidTimes();
            //            string[] AcademyCiy = EditAcademyViewModel.AcademyFullName.Split("/");
            //            EditAcademyViewModel.CityName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();
            //            return Page();
            //        }
            //    }

            //}

            for (int i = 0; i < compareList.Count; i++)
            {
                if (_courseService.IsValidTimesAcademyInCourse(AcademyId, compareList[i]))
                {
                    ModelState.AddModelError("EditAcademyViewModel.AcademyName", "از ساعت های حذف شده در قسمت کلاس ها استفاده شده است. لطفا ابتدا ساعت کلاس ویرایش و سپس  از طریق ادمین حذف شود.");
                    EditAcademyViewModel = _userService.GetAcademyForEdit(AcademyId);
                    ViewData["Type"] = _userService.GetAcademyTypes();
                    ViewData["Valid"] = _userService.GetAcademyValidTimes();
                    string[] AcademyCiy = EditAcademyViewModel.AcademyFullName.Split("/");
                    EditAcademyViewModel.CityName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();
                    List<BBBServers> servers = new List<BBBServers>();
                    servers = _userService.GetServers();
                    ViewData["ServerList"] = servers;
                    return Page();
                }

            }

            int AcId = _userService.EditAcademyFromAdmin(EditAcademyViewModel, AcademyId);
            _userService.UpdateCourseServer(AcId, EditAcademyViewModel.BBBServerId);
            _userService.EditTypesforAcademy(selectedTypes, AcId);
            _userService.EditValidTimesForAcademy(selectedTimes, AcId);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 5, DateTime.Now, remoteIpAddress.ToString(),
                "اطلاعات آموزشگاه رانندگی " + EditAcademyViewModel.AcademyFullName + " توسط " + User.Identity.Name + "بروز رسانی گردید");
            return RedirectToPage("Index");
        }
    }
}