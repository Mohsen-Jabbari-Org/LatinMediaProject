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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace LatinMedia.Web.Pages.Leon.Report
{
    [UserRoleChecker]
    [PermissionChecker(49)]
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public ReportForAdminViewModel ReportForAdminViewModel { get; set; }
        public void OnGet()
        {
            ReportForAdminViewModel = _courseService.GetReportForAdmin();
        }

        public IActionResult OnPost(string stDate = "", string fhDate = "")
        {
            DateTime CreateDate = new DateTime();
            DateTime EndDate = new DateTime();
            PersianCalendar pc = new PersianCalendar();
            PersianCalendar pcEnd = new PersianCalendar();
            string dateTemp = string.Empty;
            // 12/20/1397
            if (!string.IsNullOrEmpty(stDate) && !string.IsNullOrEmpty(fhDate))
            {
                string time = "00:00";
                string endtime = "23:59";
                string tDate = FixedText.FixedTxt(stDate);
                string hDate = FixedText.FixedTxt(fhDate);
                string[] std = tDate.Split("/");
                string[] htd = hDate.Split("/");
                DateTime dt = new DateTime(int.Parse(std[2]), int.Parse(std[0]), int.Parse(std[1]), int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(3, 2)), 0, pc);
                DateTime dtEnd = new DateTime(int.Parse(htd[2]), int.Parse(htd[0]), int.Parse(htd[1]), int.Parse(endtime.Substring(0, 2)), int.Parse(endtime.Substring(3, 2)), 0, pcEnd);
                CreateDate = Convert.ToDateTime(dt.ToString(CultureInfo.InvariantCulture));
                EndDate = Convert.ToDateTime(dtEnd.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                ReportForAdminViewModel = _courseService.GetReportForAdmin();
                return Page();
            }
            ViewData["start"] = stDate;
            ViewData["end"] = fhDate;
            ReportForAdminViewModel = _courseService.GetReportForAdmin(CreateDate,EndDate);
            
            return Page();
        }
    }
}