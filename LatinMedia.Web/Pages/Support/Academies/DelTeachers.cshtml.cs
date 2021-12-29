using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Support.Academies
{
    public class DelTeachersModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public DelTeachersModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public TeacherAcademy TeacherAcademy { get; set; }
        public NewTeacher NewTeacher { get; set; }

        public LatinMedia.DataLayer.Entities.User.Academy Academy { get; set; }
        public void OnGet(int AcademyId)
        {
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);
            ViewData["StateId"] = stateId;
            Academy = _userService.GetAcademy(AcademyId);
            string[] AcademyCiy = Academy.AcademyFullName.Split("/");
            Academy.AcademyFullName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();

            var users = _courseService.GetTeachersForSupportToAddAcademy(stateId, AcademyId);
            ViewData["TeacherAcademy"] = new SelectList(users, "Value", "Text");

            var companies = _courseService.GetAcademiesForSupportCourse(stateId);
            ViewData["Academy"] = new SelectList(companies, "Value", "Text", AcademyId);

            ViewData["TeacherIn"] = _courseService.GetAcademiesTeachers(AcademyId);
        }

        public IActionResult OnPost(int AcademyId, int Teachers)
        {
            NewTeacher = _userService.GetTeacherByTaId(Teachers);
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);
            ViewData["StateId"] = stateId;
            if (Teachers == 0)
                return RedirectToPage("DelTeachers");
            bool removeStatus = _courseService.RemoveTeacherFromAcademy(Teachers, AcademyId);
            if(removeStatus)
            {
                ViewData["TeacherIn"] = _courseService.GetAcademiesTeachers(AcademyId);
                
                var academy = _userService.GetAcademy(AcademyId);
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 6, DateTime.Now, remoteIpAddress.ToString(),
                    "مدرس " + NewTeacher.FirstName + " " + NewTeacher.LastName + " به شماره همراه " + NewTeacher.Mobile + " توسط "
                    + User.Identity.Name + " از آموزشگاه رانندگی " + academy.AcademyFullName + " حذف گردید");
                return RedirectToPage("DelTeachers");
            }
            else
            {
                ModelState.AddModelError("Academy.AcademyName", "این مدرس در این آموزشگاه دارای کلاس فعال می باشد و امکان حذف آن وجود ندارد");
                Academy = _userService.GetAcademy(AcademyId);
                string[] AcademyCiy = Academy.AcademyFullName.Split("/");
                Academy.AcademyFullName = AcademyCiy[0].Trim() + " / " + AcademyCiy[1].Trim();
                var users = _courseService.GetTeachersForSupportToAddAcademy(stateId, AcademyId);
                ViewData["TeacherAcademy"] = new SelectList(users, "Value", "Text");
                var companies = _courseService.GetAcademiesForSupportCourse(stateId);
                ViewData["Academy"] = new SelectList(companies, "Value", "Text", AcademyId);
                ViewData["TeacherIn"] = _courseService.GetAcademiesTeachers(AcademyId);
                return Page();
            }
        }
    }
}