using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Report
{
    [UserRoleChecker]
    [PermissionChecker(50)]
    public class FinishedClassReportModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}