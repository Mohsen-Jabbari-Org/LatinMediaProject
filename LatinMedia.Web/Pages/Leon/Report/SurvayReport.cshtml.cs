using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Survay;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Pages.Leon.SurvayReport
{
    //[UserRoleChecker]
    //[PermissionChecker(49)]
    public class SurvayReportModel : PageModel
    {
        private ICourseService _courseService;
        private IUserService _userService;

        public SurvayReportModel(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [BindProperty]
        public ReportForSurvayViewModel ReportForSurvayViewModel { get; set; }

        [BindProperty]
        public List<ReportForSurvayDetailViewModel> ReportForSurvayDetailViewModels { get; set; }

        [BindProperty]
        public PagingForSurvayDetailViewModel paging { get; set; }
        public List<State> States { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Education> Educations { get; set; }
        public List<Employment> Employments { get; set; }
        public State State { get; set; }
        public void OnGet(int pageId = 1, int take = 5, string stDate = "", string fhDate = "")

        {
            ReportForSurvayViewModel = new ReportForSurvayViewModel();
            if (Request.Query.ContainsKey("SI"))
            {
                string s = Request.Query["SI"];
                if (s != "")
                {
                    ViewData["States"] = Request.Query["SI"];
                    ReportForSurvayViewModel.StateId = Convert.ToInt32(Request.Query["SI"]);
                }
            }

            if (Request.Query.ContainsKey("SD"))
            {
                string s = Request.Query["SD"];
                if (s != "")
                {
                    ViewData["start"] = Request.Query["SD"];
                    stDate = Request.Query["SD"];
                }
            }

            if (Request.Query.ContainsKey("SE"))
            {
                string s = Request.Query["SE"];
                if (s != "")
                {
                    ViewData["end"] = Request.Query["SE"];
                    fhDate = Request.Query["SE"];
                }
            }

            if (Request.Query.ContainsKey("GI"))
            {
                string s = Request.Query["GI"];
                if (s != "")
                {
                    ViewData["GI"] = Request.Query["GI"];
                    ReportForSurvayViewModel.GenderId = Convert.ToInt32(Request.Query["GI"]);
                }
            }

            if (Request.Query.ContainsKey("EDI"))
            {
                string s = Request.Query["EDI"];
                if (s != "")
                {
                    ViewData["EDI"] = Request.Query["EDI"];
                    ReportForSurvayViewModel.EduId = Convert.ToInt32(Request.Query["EDI"]);
                }
            }

            if (Request.Query.ContainsKey("EMI"))
            {
                string s = Request.Query["EMI"];
                if (s != "")
                {
                    ViewData["EMI"] = Request.Query["EMI"];
                    ReportForSurvayViewModel.EmpId = Convert.ToInt32(Request.Query["EMI"]);
                }
            }
            paging = new PagingForSurvayDetailViewModel();
            List<ReportForSurvayDetailViewModel> viewModels = new List<ReportForSurvayDetailViewModel>();
            if (!string.IsNullOrEmpty(stDate) && !string.IsNullOrEmpty(fhDate))
            {
                PersianCalendar pc = new PersianCalendar();
                PersianCalendar pcEnd = new PersianCalendar();
                string dateTemp = string.Empty;
                DateTime Sdate, Edate;
                string time = "00:00";
                string endtime = "23:59";
                string tDate = FixedText.FixedTxt(stDate);
                string hDate = FixedText.FixedTxt(fhDate);
                string[] std = tDate.Split("/");
                string[] htd = hDate.Split("/");
                DateTime dt = new DateTime(int.Parse(std[2]), int.Parse(std[0]), int.Parse(std[1]), int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0, pc);
                DateTime dtEnd = new DateTime(int.Parse(htd[2]), int.Parse(htd[0]), int.Parse(htd[1]), int.Parse(endtime.Substring(0, 2)), int.Parse(endtime.Substring(3, 2)), 0, pcEnd);
                Sdate = Convert.ToDateTime(dt.ToString(CultureInfo.InvariantCulture));
                Edate = Convert.ToDateTime(dtEnd.ToString(CultureInfo.InvariantCulture));
                string s = Sdate.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                string e = Edate.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayViewModel.StartDate = s;
                ReportForSurvayViewModel.EndDate = e;
                ReportForSurvayDetailViewModels = _courseService.GetReportForSurvay(paging, ReportForSurvayViewModel, pageId, take);
                States = _courseService.GetStates();
                ViewData["States"] = new SelectList(States, "StateId", "StateName");
                Genders = _userService.GetGenders();
                ViewData["Gender"] = new SelectList(Genders, "GenderId", "GenderName");
                Educations = _userService.GetEducations();
                ViewData["Education"] = new SelectList(Educations, "EduId", "EduName");
                Employments = _userService.GetEmployments();
                ViewData["Employment"] = new SelectList(Employments, "EmpId", "EmpName");
                ViewData["end"] = fhDate;
                ViewData["startPrint"] = s;
                ViewData["endPrint"] = e;
                ViewData["start"] = stDate;
                ViewData["GI"] = ReportForSurvayViewModel.GenderId;
                ViewData["EDI"] = ReportForSurvayViewModel.EduId;
                ViewData["EMI"] = ReportForSurvayViewModel.EmpId;
                ViewData["PageID"] = (pageId - 1) * take + 1;
                if (pageId > 1 && pageId != paging.PageCount)
                {
                    ViewData["Take"] = ((pageId - 1) * take) + take;
                }
                else if (pageId == paging.PageCount)
                {
                    ViewData["Take"] = ((pageId - 1) * take) + (paging.UserCounts % take);
                }
                else
                {
                    ViewData["Take"] = take;
                }
            }
            else
            {
                ReportForSurvayViewModel.StartDate = DateTime.Now.AddHours(-10).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayViewModel.EndDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayDetailViewModels = _courseService.GetReportForSurvay(paging, ReportForSurvayViewModel, 1, 10);
                States = _courseService.GetStates();
                ViewData["States"] = new SelectList(States, "StateId", "StateName");
                Genders = _userService.GetGenders();
                ViewData["Gender"] = new SelectList(Genders, "GenderId", "GenderName");
                Educations = _userService.GetEducations();
                ViewData["Education"] = new SelectList(Educations, "EduId", "EduName");
                Employments = _userService.GetEmployments();
                ViewData["Employment"] = new SelectList(Employments, "EmpId", "EmpName");
            }
        }

        public IActionResult OnPost(int pageId = 1, int take = 5,string stDate = "", string fhDate = "")
        {
            if (Request.Query.ContainsKey("SI"))
            {
                string s = Request.Query["SI"];
                if (s != "")
                    ViewData["States"] = Request.Query["SI"];
            }

            if (Request.Query.ContainsKey("SD"))
            {
                string s = Request.Query["SD"];
                if (s != "")
                    ViewData["start"] = Request.Query["SD"];
            }

            if (Request.Query.ContainsKey("SE"))
            {
                string s = Request.Query["SE"];
                if (s != "")
                    ViewData["end"] = Request.Query["SE"];
            }

            if (Request.Query.ContainsKey("GI"))
            {
                string s = Request.Query["GI"];
                if (s != "")
                {
                    ViewData["GI"] = Request.Query["GI"];
                    ReportForSurvayViewModel.GenderId = Convert.ToInt32(Request.Query["GI"]);
                }
            }

            if (Request.Query.ContainsKey("EDI"))
            {
                string s = Request.Query["EDI"];
                if (s != "")
                {
                    ViewData["EDI"] = Request.Query["EDI"];
                    ReportForSurvayViewModel.EduId = Convert.ToInt32(Request.Query["EDI"]);
                }
            }

            if (Request.Query.ContainsKey("EMI"))
            {
                string s = Request.Query["EMI"];
                if (s != "")
                {
                    ViewData["EMI"] = Request.Query["EMI"];
                    ReportForSurvayViewModel.EmpId = Convert.ToInt32(Request.Query["EMI"]);
                }
            }

            paging = new PagingForSurvayDetailViewModel();
            List<ReportForSurvayDetailViewModel> viewModels = new List<ReportForSurvayDetailViewModel>();
            if (!string.IsNullOrEmpty(stDate) && !string.IsNullOrEmpty(fhDate))
            {
                PersianCalendar pc = new PersianCalendar();
                PersianCalendar pcEnd = new PersianCalendar();
                string dateTemp = string.Empty;
                DateTime Sdate, Edate;
                string time = "00:00";
                string endtime = "23:59";
                string tDate = FixedText.FixedTxt(stDate);
                string hDate = FixedText.FixedTxt(fhDate);
                string[] std = tDate.Split("/");
                string[] htd = hDate.Split("/");
                DateTime dt = new DateTime(int.Parse(std[2]), int.Parse(std[0]), int.Parse(std[1]), int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0, pc);
                DateTime dtEnd = new DateTime(int.Parse(htd[2]), int.Parse(htd[0]), int.Parse(htd[1]), int.Parse(endtime.Substring(0, 2)), int.Parse(endtime.Substring(3, 2)), 0, pcEnd);
                Sdate = Convert.ToDateTime(dt.ToString(CultureInfo.InvariantCulture));
                Edate = Convert.ToDateTime(dtEnd.ToString(CultureInfo.InvariantCulture));
                string s = Sdate.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                string e = Edate.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayViewModel.StartDate = s;
                ReportForSurvayViewModel.EndDate = e;
                ReportForSurvayDetailViewModels = _courseService.GetReportForSurvay(paging, ReportForSurvayViewModel, pageId, take);
                States = _courseService.GetStates();
                ViewData["States"] = new SelectList(States, "StateId", "StateName");
                Genders = _userService.GetGenders();
                ViewData["Gender"] = new SelectList(Genders, "GenderId", "GenderName");
                Educations = _userService.GetEducations();
                ViewData["Education"] = new SelectList(Educations, "EduId", "EduName");
                Employments = _userService.GetEmployments();
                ViewData["Employment"] = new SelectList(Employments, "EmpId", "EmpName");
                ViewData["end"] = fhDate;
                ViewData["startPrint"] = s;
                ViewData["endPrint"] = e;
                ViewData["start"] = stDate;
                ViewData["GI"] = ReportForSurvayViewModel.GenderId;
                ViewData["EDI"] = ReportForSurvayViewModel.EduId;
                ViewData["EMI"] = ReportForSurvayViewModel.EmpId;
                ViewData["PageID"] = (pageId - 1) * take + 1;
                if (pageId > 1 && pageId != paging.PageCount)
                {
                    ViewData["Take"] = ((pageId - 1) * take) + take;
                }
                else if (pageId == paging.PageCount)
                {
                    ViewData["Take"] = ((pageId - 1) * take) + (paging.UserCounts % take);
                }
                else
                {
                    ViewData["Take"] = take;
                }
                return Page();
            }
            else
            {
                ReportForSurvayViewModel.StartDate = DateTime.Now.AddHours(-10).ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayViewModel.EndDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
                ReportForSurvayDetailViewModels = _courseService.GetReportForSurvay(paging, ReportForSurvayViewModel, 1, 10);
                States = _courseService.GetStates();
                ViewData["States"] = new SelectList(States, "StateId", "StateName");
                Genders = _userService.GetGenders();
                ViewData["Gender"] = new SelectList(Genders, "GenderId", "GenderName");
                Educations = _userService.GetEducations();
                ViewData["Education"] = new SelectList(Educations, "EduId", "EduName");
                Employments = _userService.GetEmployments();
                ViewData["Employment"] = new SelectList(Employments, "EmpId", "EmpName");
                return Page();
            }
        }
    }
}