using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Genertors;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Controllers
{
    public class LearningContentController : Controller
    {
        private IUserService _userService;
        private IHostingEnvironment environment;
        private ICourseService _courseService;

        public LearningContentController(IUserService userService, IHostingEnvironment environment, ICourseService courseService)
        {
            _userService = userService;
            this.environment = environment;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            //return View(_courseService.GetCourses().Item1);
            return View("index");
        }

        public IActionResult Error()
        {
            return View("Error");
        }

        public IActionResult Intro()
        {
            return View("Intro");
        }

        public IActionResult EnterClass()
        {
            return View("EnterClass");
        }

        public IActionResult EnterTeacherClass()
        {
            return View("EnterTeacherClass");
        }

        public IActionResult EnterClassMobile()
        {
            return View("EnterClassMobile");
        }

        public IActionResult HafteNaja()
        {
            return View("HafteNaja");
        }
    }
}