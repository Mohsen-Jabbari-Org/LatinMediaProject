using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Areas.TeacherPanel.Controllers
{
    [Area("TeacherPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        private IOrderService _orderService;
        private ICourseService _courseService;
        private IHostingEnvironment _environment;

        public MyOrdersController(IOrderService orderService, ICourseService courseService, IHostingEnvironment environment)
        {
            _orderService = orderService;
            _courseService = courseService;
            _environment = environment;
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
                return Redirect("/TeacherPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }

            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId, string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
            return Redirect("/TeacherPanel/MyOrders/ShowOrder/" + orderId + "?type=" + type);
        }

        public IActionResult UserCourses()
        {
            return View(_orderService.GetCoursesForDownload(User.Identity.GetMobile()));
        }

        public IActionResult TeacherCourses()
        {
            return View(_orderService.GetCoursesForTeacher(User.Identity.GetMobile()));
        }

        public IActionResult TeacherFinishedCourses()
        {
            return View(_orderService.GetFinishedCoursesForTeacher(User.Identity.GetMobile()));
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