using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;

        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }


        public IActionResult Index(int pageId = 1, string filter = ""
            , int type = 0, int minPrice = 0, int maxPrice = 0,
            List<int> selectedGroups = null, int companyId = 0 , int teacherId = 0)
        {
            ViewBag.Groups = _courseService.GetAllGroups();
            ViewBag.CourseTypes = _courseService.GetAllCourseTypes();
            ViewBag.Companies = _courseService.GetAllCompanies();
            ViewBag.Teachers = _courseService.GetAllTeachers();
            ViewBag.PageId = pageId;
            //--------------------------
            ViewBag.SelectedType = type;
            ViewBag.SelectedGroups = selectedGroups;
            ViewBag.SelectedCompany = companyId;
            ViewBag.SelectedTeacher = teacherId;

            ViewData["SelectedGroup"] = selectedGroups;
            var result = _courseService.GetCourses(pageId, filter, type, minPrice, maxPrice, selectedGroups, 6 , companyId , teacherId);
            return View("Index",result);
        }

        //public IActionResult Companies()
        //{
        //    return View(_courseService.ShowAllCompanies());
        //}

        public IActionResult Teachers()
        {
            return View(_courseService.ShowAllTeachers());
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity.GetMobile(), id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId);
        }

        public IActionResult CreateComment(CourseComment coment)
        {
            coment.UserId = _userService.GetUserIdByMobile(User.Identity.GetMobile());
            coment.CreateDate = DateTime.Now;
            coment.IsDelete = false;
            coment.IsReadAdmin = false;
            _courseService.AddComment(coment);
            return View("ShowComments", _courseService.GetCourseComments(coment.CourseId));
        }


        public IActionResult ShowComments(int id, int pageId = 1)
        {
            return View(_courseService.GetCourseComments(id, pageId));
        }
    }
}