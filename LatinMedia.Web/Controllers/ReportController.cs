using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Genertors;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace LatinMedia.Web.Controllers
{
    public class ReportController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public ReportController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
            Stimulsoft.Base.StiLicense.Key = @"6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OJN40hxJjK5JbrxU+NrJ3E0OUAve6MDSIxK3504G4vSTqZezogz9ehm+xS8zUyh3tFhCWSvIoPFEEuqZTyO744uk+ezyGDj7C5jJQQjndNuSYeM+UdsAZVREEuyNFHLm7gD9OuR2dWjf8ldIO6Goh3h52+uMZxbUNal/0uomgpx5NklQZwVfjTBOg0xKBLJqZTDKbdtUrnFeTZLQXPhrQA5D+hCvqsj+DE0n6uAvCB2kNOvqlDealr9mE3y978bJuoq1l4UNE3EzDk+UqlPo8KwL1XM+o1oxqZAZWsRmNv4Rr2EXqg/RNUQId47/4JO0ymIF5V4UMeQcPXs9DicCBJO2qz1Y+MIpmMDbSETtJWksDF5ns6+B0R7BsNPX+rw8nvVtKI1OTJ2GmcYBeRkIyCB7f8VefTSOkq5ZeZkI8loPcLsR4fC4TXjJu2loGgy4avJVXk32bt4FFp9ikWocI9OQ7CakMKyAF6Zx7dJF1nZw";
        }

        public IActionResult Index()
        {
            //return View(_courseService.GetCourses().Item1);
            return View("index");
        }

        public IActionResult ShowPrint()
        {
            return View("ShowPrint");
        }

        public IActionResult ShowSurvayPrint()
        {
            return View("ShowSurvayPrint");
        }

        public IActionResult SurvayPrint()
        {
            int StateId = 0 , GenderId = 0 , EduId = 0 , EmpId = 0;
            string stDate=DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff"), fhDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");

            if (Request.Query.ContainsKey("SI"))
            {
                string s = Request.Query["SI"];
                if (s != "")
                {
                    StateId = Convert.ToInt32(Request.Query["SI"]);
                }
            }

            if (Request.Query.ContainsKey("SD"))
            {
                string s = Request.Query["SD"];
                if (s != "")
                {
                    stDate = string.Empty;
                    stDate = Request.Query["SD"];
                }
            }

            if (Request.Query.ContainsKey("SE"))
            {
                string s = Request.Query["SE"];
                if (s != "")
                {
                    fhDate = string.Empty;
                    fhDate = Request.Query["SE"];
                }
            }

            if (Request.Query.ContainsKey("GI"))
            {
                string s = Request.Query["GI"];
                if (s != "")
                {
                    GenderId = Convert.ToInt32(Request.Query["GI"]);
                }
            }

            if (Request.Query.ContainsKey("EDI"))
            {
                string s = Request.Query["EDI"];
                if (s != "")
                {
                    EduId = Convert.ToInt32(Request.Query["EDI"]);
                }
            }

            if (Request.Query.ContainsKey("EMI"))
            {
                string s = Request.Query["EMI"];
                if (s != "")
                {
                    EmpId = Convert.ToInt32(Request.Query["EMI"]);
                }
            }
            StiReport report = new StiReport();
            report.Load("wwwroot/Rpt/SurvayReport.mrt");
            var ReportForAdminViewModel = _courseService.GetReportForSurvay(StateId, stDate, fhDate , GenderId , EmpId , EduId);
            report.RegData("SurvayDetailsDt", ReportForAdminViewModel);
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ShowSurvayTeacherPrint()
        {
            return View("ShowSurvayTeacherPrint");
        }

        public IActionResult ShowSurvayTeacherCommentPrint()
        {
            return View("ShowSurvayTeacherCommentPrint");
        }

        public IActionResult SurvayCommentTeacherPrint()
        {
            int TeacherId = 0, GenderId = 0, EduId = 0, EmpId = 0;
            string stDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff"), fhDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
            List<ReportForSurvayCommentsTeachersViewModel> Tmodel = new List<ReportForSurvayCommentsTeachersViewModel>();
            List<ReportForSurvayCommentsTeachersViewModel> reportForSurvayCommentsTeachersViewModels = new List<ReportForSurvayCommentsTeachersViewModel>();

            if (Request.Query.ContainsKey("TI"))
            {
                string s = Request.Query["TI"];
                if (s != "")
                {
                    TeacherId = Convert.ToInt32(Request.Query["TI"]);
                }
            }

            if (Request.Query.ContainsKey("SD"))
            {
                string s = Request.Query["SD"];
                if (s != "")
                {
                    stDate = string.Empty;
                    stDate = Request.Query["SD"];
                }
            }

            if (Request.Query.ContainsKey("SE"))
            {
                string s = Request.Query["SE"];
                if (s != "")
                {
                    fhDate = string.Empty;
                    fhDate = Request.Query["SE"];
                }
            }

            if (Request.Query.ContainsKey("GI"))
            {
                string s = Request.Query["GI"];
                if (s != "")
                {
                    GenderId = Convert.ToInt32(Request.Query["GI"]);
                }
            }

            if (Request.Query.ContainsKey("EDI"))
            {
                string s = Request.Query["EDI"];
                if (s != "")
                {
                    EduId = Convert.ToInt32(Request.Query["EDI"]);
                }
            }

            if (Request.Query.ContainsKey("EMI"))
            {
                string s = Request.Query["EMI"];
                if (s != "")
                {
                    EmpId = Convert.ToInt32(Request.Query["EMI"]);
                }
            }
            StiReport report = new StiReport();
            report.Load("wwwroot/Rpt/SurvayTeacherCommentReport.mrt");

            Tmodel = _courseService.GetCommentForTeacherSurvay(TeacherId, stDate, fhDate, GenderId, EmpId, EduId);
            report.RegData("SurvayTeacherCommentsDt", Tmodel);
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult SurvayTeacherPrint()
        {
            int TeacherId = 0, GenderId = 0, EduId = 0, EmpId = 0;
            string stDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff"), fhDate = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffffff");
            List<ReportForSurvayDetailTeachersViewModel> Tmodel = new List<ReportForSurvayDetailTeachersViewModel>();
            List<ReportForSurvayDetailTeachersViewModel> ReportForSurvayDetailTeachersViewModels = new List<ReportForSurvayDetailTeachersViewModel>();

            if (Request.Query.ContainsKey("TI"))
            {
                string s = Request.Query["TI"];
                if (s != "")
                {
                    TeacherId = Convert.ToInt32(Request.Query["TI"]);
                }
            }

            if (Request.Query.ContainsKey("SD"))
            {
                string s = Request.Query["SD"];
                if (s != "")
                {
                    stDate = string.Empty;
                    stDate = Request.Query["SD"];
                }
            }

            if (Request.Query.ContainsKey("SE"))
            {
                string s = Request.Query["SE"];
                if (s != "")
                {
                    fhDate = string.Empty;
                    fhDate = Request.Query["SE"];
                }
            }

            if (Request.Query.ContainsKey("GI"))
            {
                string s = Request.Query["GI"];
                if (s != "")
                {
                    GenderId = Convert.ToInt32(Request.Query["GI"]);
                }
            }

            if (Request.Query.ContainsKey("EDI"))
            {
                string s = Request.Query["EDI"];
                if (s != "")
                {
                    EduId = Convert.ToInt32(Request.Query["EDI"]);
                }
            }

            if (Request.Query.ContainsKey("EMI"))
            {
                string s = Request.Query["EMI"];
                if (s != "")
                {
                    EmpId = Convert.ToInt32(Request.Query["EMI"]);
                }
            }
            StiReport report = new StiReport();
            report.Load("wwwroot/Rpt/SurvayTeacherReport.mrt");

            int[] AcademyList = _courseService.GetTeachersAcademyForSurvayPrint(TeacherId, stDate, fhDate);
            for (int i = 0; i < AcademyList.Length; i++)
            {
                Tmodel = _courseService.GetReportForTeacherSurvay(TeacherId, AcademyList[i], stDate, fhDate, GenderId, EmpId, EduId);
                ReportForSurvayDetailTeachersViewModels.Add(Tmodel[0]);
            }

            report.RegData("SurvayTeachersDetailsDt", ReportForSurvayDetailTeachersViewModels);
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult Print()
        {
            StiReport report = new StiReport();
            report.Load("wwwroot/Rpt/Report.mrt");
            var ReportForAdminViewModel = _courseService.GetReportForAdmin();
            report.RegData("dt", ReportForAdminViewModel);
            return StiNetCoreViewer.GetReportResult(this, report);
        }

        public IActionResult ViewEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }
    }
}