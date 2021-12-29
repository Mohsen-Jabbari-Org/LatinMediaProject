using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using LatinMedia.Core.Security;
using LatinMedia.DataLayer.Entities.User;
using System.Globalization;
using BigBlueButtonAPI.Core;
using Sample.Models;
using LatinMedia.DataLayer.Context;
using LatinMedia.Core.Convertors;

namespace LatinMedia.Web.Pages.Leon.RhgClass
{
    [UserRoleChecker]
    [PermissionChecker(36)]
    public class CreateClassModel : PageModel
    {
        private readonly BigBlueButtonAPIClient client;
        private ICourseService _courseService;
        private IUserService _userService;
        private LatinMediaContext _context;

        public CreateClassModel(ICourseService courseService, IUserService userService, BigBlueButtonAPIClient client, LatinMediaContext context)
        {
            _courseService = courseService;
            _userService = userService;
            this.client = client;
            _context = context;
        }


        [BindProperty]
        public Course Course { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public LatinMedia.DataLayer.Entities.User.Academy Academy { get; set; }

        public void OnGet()
        {

            var groups = _courseService.GetGroupsForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            //string ac = User.Identity.GetAcademyId();
            //Int32 AcId = Int32.Parse(ac);
            //var academy = _userService.GetAcademyName(AcId);
            //ViewData["Academies"] = new SelectList(academy, "Value", "Text");

            var level = _courseService.GetLevelsForManageCourse();
            ViewData["CourseLevel"] = new SelectList(level, "Value", "Text");

            var type = _courseService.GetCourseTypesForManageCourse();
            ViewData["CourseType"] = new SelectList(type, "Value", "Text");

            var users = _courseService.GetUsersFromUsersToManageCourse();
            ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

            var company = _courseService.GetCompaniesForManageCourse();
            ViewData["CourseCompany"] = new SelectList(company, "Value", "Text");


        }

        public IActionResult OnPost(IFormFile courseFile, IFormFile imgCourseUp , string stDate = "", string fhDate = "" , int AcademyId = 0)
        {
            PersianCalendar pc = new PersianCalendar();
            PersianCalendar pcEnd = new PersianCalendar();
            PersianCalendar pcStart = new PersianCalendar();
            PersianCalendar pcFinish = new PersianCalendar();
            string dateTemp = string.Empty;
            // 12/20/1397
            if (!string.IsNullOrEmpty(stDate) && !string.IsNullOrEmpty(fhDate))
            {
                string time = _courseService.GetStartTimes(Course.VTA_Id);
                string endtime = _courseService.GetEndTimes(Course.VTA_Id);
                string tDate = FixedText.FixedTxt(stDate);
                string hDate = FixedText.FixedTxt(fhDate);
                string[] std = tDate.Split("/");
                string[] htd = hDate.Split("/");
                DateTime dt = new DateTime(int.Parse(std[2]), int.Parse(std[0]), int.Parse(std[1]), int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0, pc);
                DateTime dtEnd = new DateTime(int.Parse(htd[2]), int.Parse(htd[0]), int.Parse(htd[1]), int.Parse(endtime.Substring(0, 2)), int.Parse(endtime.Substring(3, 2)), 0, pcEnd);
                Course.CreateDate = Convert.ToDateTime(dt.ToString(CultureInfo.InvariantCulture));
                Course.EndDate = Convert.ToDateTime(dtEnd.ToString(CultureInfo.InvariantCulture));
                DateTime dtstart = new DateTime(int.Parse(std[2]), int.Parse(std[0]), int.Parse(std[1]), pcStart);
                DateTime dtFinish = new DateTime(int.Parse(htd[2]), int.Parse(htd[0]), int.Parse(htd[1]), pcFinish);
                if (DateTime.Compare(Convert.ToDateTime(dtstart.ToString(CultureInfo.InvariantCulture)),
                    Convert.ToDateTime(dtFinish.ToString(CultureInfo.InvariantCulture))) == 0)
                {
                    Course.CourseFaTitle = _userService.GetAcademy(Course.CompanyId).AcademyFullName + " " + Course.CreateDate.ToNewShamsi() + " " +
                    _courseService.GetGroupNameForAdmin(Course.GroupId) + " از ساعت " + time + " تا ساعت " + endtime;

                    Course.CourseDescription = Course.CourseFaTitle;
                }
                else
                {
                    Course.CourseFaTitle = _userService.GetAcademy(Course.CompanyId).AcademyFullName + " " + Course.CreateDate.ToNewShamsi() + " الی " +
                        Course.EndDate.ToNewShamsi() + " " + _courseService.GetGroupNameForAdmin(Course.GroupId) +
                        " از ساعت " + time + " تا ساعت " + endtime;

                    Course.CourseDescription = Course.CourseFaTitle;
                }
            }
            else
            {
                Course.CreateDate = DateTime.Now;
                Course.EndDate = DateTime.Now;
            }
            if (Course.TeacherId == 0)
            {
                stDate = Course.CreateDate.ToString();
                ModelState.AddModelError("Course.Teacher", "مدرس انتخاب نشده است");
            }

            if (!ModelState.IsValid)
            {
                

                var groups = _courseService.GetGroupsForManageCourse();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text");

                string ac = User.Identity.GetAcademyId();
                Int32 AcId = Int32.Parse(ac);
                var academy = _userService.GetAcademyName(AcId);
                ViewData["Academies"] = new SelectList(academy, "Value", "Text");

                var level = _courseService.GetLevelsForManageCourse();
                ViewData["CourseLevel"] = new SelectList(level, "Value", "Text");

                var type = _courseService.GetCourseTypesForManageCourse();
                ViewData["CourseType"] = new SelectList(type, "Value", "Text");

                var teacher = _courseService.GetTeachersFromUsersToManageCourse(AcId);
                ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text");

                var users = _courseService.GetUsersFromUsersToManageCourse();
                ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

                var company = _courseService.GetCompaniesForManageCourse();
                ViewData["CourseCompany"] = new SelectList(company, "Value", "Text");

                var validTimes = _courseService.GetValidTimesForManageCourse(AcId);
                ViewData["ValidTimes"] = new SelectList(validTimes, "Value", "Text");
                return Page();
            }
            Course.DemoFileName = Guid.NewGuid().ToString().Replace("-","");

            #region Create bbb Class Link

            var Id = Course.DemoFileName;
                var model = new StartModel
                {
                    Id = Id,
                    Url = "https://lms.btnrahgosha.ir/Zoom/Join/?id=" + Id,
                    //Url = "https://localhost:44341/Zoom/Join/?id=" + Id,
                    Name = ""
                };
            Course.CourseLatinTitle = model.Url;
            Course.Language = "https://lms.btnrahgosha.ir/Zoom/Start/?id=" + model.Id;
            //Course.Language = "https://localhost:44341/Zoom/Start/?id=" + model.Id;
            #endregion
            //var server = _courseService.GetServerParameters(Course.CompanyId);
            Course.ServerId = 2;
            _courseService.AddCourse(Course, imgCourseUp, courseFile);
            TempData["InsertCourse"] = true;
            return RedirectToPage("Index");
        }
    }
}