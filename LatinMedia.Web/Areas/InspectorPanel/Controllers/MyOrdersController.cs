using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Areas.InspectorPanel.Controllers
{
    [Area("InspectorPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        public BBBServers BBBServers { get; set; }
        BigBlueButtonAPISettings BigBlueButtonAPISettings = new BigBlueButtonAPISettings();
        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        private BigBlueButtonAPIClient client;
        public List<BBBServers> BBBServerList { get; set; }
        private IOrderService _orderService;
        private ICourseService _courseService;
        private IHostingEnvironment _environment;

        public MyOrdersController(BigBlueButtonAPIClient client ,IOrderService orderService, ICourseService courseService, 
            IHostingEnvironment environment)
        {
            BigBlueButtonAPISettings.ServerAPIUrl = "https://km2.btnazmoon.ir/bigbluebutton/api/";
            BigBlueButtonAPISettings.SharedSecret = "nOhG82ZwcmDQMXWlBnenjdtuFe9c9QVzOGAkzSR5s";
            UrlBuilder UrlBuilder = new UrlBuilder(BigBlueButtonAPISettings);
            //this.client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
            _orderService = orderService;
            _courseService = courseService;
            _environment = environment;
            BBBServerList = _courseService.GetAllServers();
        }
        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.GetMobile()));
        }

        public IActionResult ShowOrder(int id , bool finaly = false, string type = "")
        {
            var order = _orderService.GetOrdersForUserPanel(User.Identity.GetMobile(), id);
            if(order==null)
            {
                return NotFound();
            }
            ViewBag.Finaly = finaly;
            ViewBag.DiscountType = type;
            return View(order);
        }

        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinalyOrder(User.Identity.GetMobile(), id))
            {
                return Redirect("/InspectorPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }

            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId, string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
            return Redirect("/InspectorPanel/MyOrders/ShowOrder/" + orderId + "?type=" + type);
        }

        public IActionResult UserCourses()
        {
            return View(_orderService.GetCoursesForDownload(User.Identity.GetMobile()));
        }

        public IActionResult ActiveCourses()
        {
            //لیست جلسات آنلاین تمام سرورها استخراج و با آی دی کلاس های اون استان مقایسه شود
            int ServerCount = BBBServerList.Count;
            List<BigBlueButtonAPIClient> clients = new List<BigBlueButtonAPIClient>();
            List<string> results = new List<string>();
            ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
            GetMeetingsResponse result = new GetMeetingsResponse();
            for (int i = 0; i < ServerCount;i++)
            {
                BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                BigBlueButtonAPISettings.SharedSecret = string.Empty;
                BigBlueButtonAPISettings.ServerAPIUrl = BBBServerList[i].ServerUrl;
                BigBlueButtonAPISettings.SharedSecret = BBBServerList[i].ServerSharesSecret;
                client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
                var r = client.GetMeetingsAsync();
                int meetingsCount;
                try
                {
                    meetingsCount = r.Result.meetings.Count;
                }
                catch
                {
                    meetingsCount = 0;
                }
                //if (r.Result != null && r.Result.meetings.Count != 0)
                if (meetingsCount != 0)
                {
                    for (int j = 0; j < r.Result.meetings.Count; j++)
                    {
                        if (r.Result.meetings[j].running == true)
                        {
                            results.Add(r.Result.meetings[j].meetingID);
                        }
                    }
                }
            }
            return View(_orderService.GetCoursesForInspector(User.Identity.GetMobile(), results));
        }

        [Route("DownloadFile/{courseId}")]
        public IActionResult DownloadFile(int courseId)
        {
            var course = _courseService.GetCourseById(courseId);
            if (course != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (_orderService.IsUserInCourse(User.Identity.GetMobile(), course.CourseId))
                    {
                        string filePath = Path.Combine(_environment.WebRootPath, "course/download",
                            course.CourseFileName);
                        string fileName = course.CourseFileName;
                        byte[] file = System.IO.File.ReadAllBytes(filePath);
                        return File(file, "application/force-download", fileName);

                    }
                }
            }

            return Forbid();
        }
    }
}