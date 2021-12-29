using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Support.Academies
{
    public class IndexModel : PageModel
    {
        private IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public AcademiesForAdminViewModel AcademiesForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, int take = 5, string filterByName = "", string filterByCity = "", string filterByCityName = "")
        {
            string AcId = User.Identity.GetAcademyId();
            int stateId = _userService.GetStateFoAcademy(_userService.GetAcademy(Convert.ToInt32(AcId)).CityId);

            if (Request.Query.ContainsKey("fn"))
            {
                string s = Request.Query["fn"];
                if (s != "")
                    filterByName = Request.Query["fn"];
            }

            if (Request.Query.ContainsKey("fc"))
            {
                string s = Request.Query["fc"];
                if (s != "")
                    filterByCity = Request.Query["fc"];
            }

            if (Request.Query.ContainsKey("fcn"))
            {
                string s = Request.Query["fcn"];
                if (s != "")
                    filterByCityName = Request.Query["fcn"];
            }

            ViewData["FilterName"] = filterByName;
            ViewData["FilterCity"] = filterByCity;
            ViewData["FilterCityName"] = filterByCityName;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            AcademiesForAdminViewModel = _userService.GetProvinceAcademies(stateId, pageId, take, filterByName, filterByCity);
            ViewData["StateId"] = stateId;

            if (pageId > 1 && pageId != AcademiesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + take;
            }
            else if (pageId == AcademiesForAdminViewModel.PageCount)
            {
                ViewData["Take"] = ((pageId - 1) * take) + (AcademiesForAdminViewModel.AcademyCounts % take);
            }
            else
            {
                ViewData["Take"] = take;
            }
            
        }
    }
}