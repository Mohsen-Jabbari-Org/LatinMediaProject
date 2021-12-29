using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Support.RhgClass
{
    public class EditClassModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public EditClassModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }


        [BindProperty]
        public Course Course { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public LatinMedia.DataLayer.Entities.User.Academy Academy { get; set; }

        public void OnGet(int id)
        {
            int StateId = _userService.GetStateFoAcademy(_userService.GetCityForAcademies(Convert.ToInt32(User.Identity.GetAcademyId())));
            ViewData["StateId"] = StateId;
            Course = _courseService.GetCourseById(id);
            int Cityid = _userService.GetCityForAcademies(Course.CompanyId);
            int Stateid = _userService.GetStateFoAcademy(Cityid);
            int Academyid = Course.CompanyId;
            ViewData["CourseAcademy"] = _userService.GetAcademyFullName(Course.CompanyId);

            #region Class Groups
            var groups = _courseService.GetGroupsForManageCourse();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

            List<SelectListItem> subGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            subGroupsList.AddRange(_courseService.GetSubGroupsForManageCourse(Course.GroupId));
            string selectedSubGroup = null;
            if (Course.SubGroup != null)
            {
                selectedSubGroup = Course.SubGroup.ToString();
            }
            ViewData["SubGroups"] = new SelectList(subGroupsList, "Value", "Text", selectedSubGroup);

            List<SelectListItem> secondSubGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };

            secondSubGroupsList.AddRange(_courseService.GetSecondSubGroupsForManageCourse(Course.SubGroupId ?? 0));
            string selectedSecondSubGroup = null;
            if (Course.SecondSubGroup != null)
            {
                selectedSecondSubGroup = Course.SecondSubGroup.ToString();
            }
            ViewData["SecondSubGroups"] = new SelectList(secondSubGroupsList, "Value", "Text", selectedSecondSubGroup);
            #endregion

            #region State , City , Academy

            var state = _userService.GetState();
            ViewData["States"] = new SelectList(state, "Value", "Text", Stateid);

            List<SelectListItem> CityList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            CityList.AddRange(_userService.GetCity(Stateid));
            string selectedCity = null;
            if (Cityid != 0)
            {
                selectedCity = Cityid.ToString();
            }
            ViewData["Cities"] = new SelectList(CityList, "Value", "Text", selectedCity);

            var academy = _userService.GetAcademies(Cityid);
            ViewData["Academies"] = new SelectList(academy, "Value", "Text", Course.CompanyId);

            #endregion

            var level = _courseService.GetLevelsForManageCourse();
            ViewData["CourseLevel"] = new SelectList(level, "Value", "Text");

            var type = _courseService.GetCourseTypesForManageCourse();
            ViewData["CourseType"] = new SelectList(type, "Value", "Text");

            var teacher = _courseService.GetTeachersFromUsersToManageCourse(Course.CompanyId);
            ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text");

            var users = _courseService.GetUsersFromUsersToManageCourse();
            ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

            var company = _courseService.GetCompaniesForManageCourse();
            ViewData["CourseCompany"] = new SelectList(company, "Value", "Text");

            var validTimes = _courseService.GetValidTimesForManageCourse(Course.CompanyId);
            ViewData["ValidTimes"] = new SelectList(validTimes, "Value", "Text");
        }

        public IActionResult OnPost(IFormFile courseFile, IFormFile imgCourseUp, string stDate = "", string fhDate = "", int AcademyId = 0)
        {
            PersianCalendar pcEnd = new PersianCalendar();
            PersianCalendar pc = new PersianCalendar();
            PersianCalendar pcStart = new PersianCalendar();
            PersianCalendar pcFinish = new PersianCalendar();
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
                    Course.CourseFaTitle = string.Empty;
                    Course.CourseDescription = string.Empty;
                    Course.CourseFaTitle = _userService.GetAcademy(Course.CompanyId).AcademyFullName + " " + Course.CreateDate.ToNewShamsi() + " " +
                    _courseService.GetGroupNameForAdmin(Course.GroupId) + " از ساعت " + time + " تا ساعت " + endtime;

                    Course.CourseDescription = Course.CourseFaTitle;
                }
                else
                {
                    Course.CourseFaTitle = string.Empty;
                    Course.CourseDescription = string.Empty;
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
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

                var subGroups = _courseService.GetSubGroupsForManageCourse(int.Parse(groups.First().Value));
                ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", Course.SubGroupId ?? 0);

                var secondSubGroups = _courseService.GetSecondSubGroupsForManageCourse(int.Parse(subGroups.First().Value));
                ViewData["SecondSubGroups"] = new SelectList(secondSubGroups, "Value", "Text", Course.SecondSubGroupId ?? 0);

                //var levels = _courseService.GetLevelsForManageCourse();
                //ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.LevelId);

                //var types = _courseService.GetCourseTypesForManageCourse();
                //ViewData["Types"] = new SelectList(types, "Value", "Text", Course.TypeId);

                var teacher = _courseService.GetTeachersFromUsersToManageCourse();
                ViewData["CourseTeacher"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

                var users = _courseService.GetUsersFromUsersToManageCourse();
                ViewData["CourseUsers"] = new SelectList(users, "Value", "Text");

                var companies = _courseService.GetCompaniesForManageCourse();
                ViewData["Companies"] = new SelectList(companies, "Value", "Text", Course.CompanyId);

                var validTimes = _courseService.GetValidTimesForManageCourse(Course.CompanyId);
                ViewData["ValidTimes"] = new SelectList(validTimes, "Value", "Text");
                return Page();
            }
            _courseService.UpdateCourse(Course, imgCourseUp, courseFile);
            TempData["UpdateCourse"] = true;
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            _userService.AddGeneralEvent(Convert.ToInt32(User.Identity.GetID()), 5, DateTime.Now, remoteIpAddress.ToString(),
                "کلاس با عنوان " + Course.CourseFaTitle + " توسط " + User.Identity.Name + " بروز رسانی گردید");
            return RedirectToPage("Index");
        }
    }
}