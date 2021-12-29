using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcademyController : ControllerBase
    {
        private IUserService _userService;
        private LatinMediaContext _context;
        public AcademyController(IUserService userService , LatinMediaContext context)
        {
            _userService = userService;
            _context = context;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var academyTitle = _context.Academies.Where(a => a.AcademyName.Contains(term)).Select(a => a.AcademyName).ToList();
                return Ok(academyTitle);
            }
            catch
            {

                return BadRequest();
            }
           
        }
    }
}