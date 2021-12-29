using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Genertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Senders;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Teacher;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        private LatinMediaContext _context;

        public AccountController(IUserService userService, IViewRenderService viewRender, LatinMediaContext context)
        {
            _userService = userService;
            _viewRender = viewRender;
            _context = context;
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var term = (from Academy in _context.Academies
                             where Academy.AcademyName.StartsWith(prefix)
                             select new
                             {
                                 label = Academy.AcademyFullName,
                                 val = Academy.AcademyId
                             }).ToList();

            return Json(term);
        }

        [HttpPost]
        public JsonResult CityAutoComplete(string prefix)
        {
            var term = _context.States.Join(
                                            _context.Cities,
                                            state => state.StateId,
                                            city => city.State.StateId,
                                            (state, city) => new
                                            {
                                                txtCity = city.CityName,
                                                txtState = state.StateName,
                                                label = state.StateName + " / " + city.CityName,
                                                val = city.CityId
                                            }).Where(c => c.txtCity.StartsWith(prefix) || c.txtState.StartsWith(prefix));

            return Json(term);
        }

        [HttpPost]
        public JsonResult SupportCityAutoComplete(string prefix, int StateId)
        {
            var term = _context.States.Join(
                                            _context.Cities,
                                            state => state.StateId,
                                            city => city.State.StateId,
                                            (state, city) => new
                                            {
                                                stateId = city.StateId,
                                                txtCity = city.CityName,
                                                txtState = state.StateName,
                                                label = state.StateName + " / " + city.CityName,
                                                val = city.CityId
                                            }).Where(c => c.stateId == StateId && (c.txtCity.StartsWith(prefix) || c.txtState.StartsWith(prefix)));

            return Json(term);
        }

        [HttpPost]
        public JsonResult TeachersAutoComplete(string prefix, int CityId , int AcademyId)
        {
            int StateId = _context.Cities.Where(c => c.CityId == CityId).Select(c => c.StateId).First();
            var Cities = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            var TeacherIds = _context.TeacherAcademies.Where(ta => ta.AcademyId == AcademyId).Select(ta => ta.TeacherId).ToArray();
            var term = (from NewTeacher in _context.NewTeachers
                        where NewTeacher.LastName.StartsWith(prefix) && Cities.Contains(NewTeacher.CityCode) && !TeacherIds.Contains(NewTeacher.TeacherId)
                        select new
                        {
                            label = NewTeacher.Gender.GenderName + " " + NewTeacher.FirstName + " " + NewTeacher.LastName,
                            val = NewTeacher.TeacherId
                        }).ToList();
            return Json(term);
        }

        [HttpPost]
        public JsonResult UsersAutoComplete(string prefix, int AcademyId , int CourseId)
        {
            //var userIds = _context.userCourses.Where(uc => uc.CourseId == CourseId).Select(uc => uc.UserId).ToArray();
            //var users = _context.UserRoles.Select(ur => ur.UserId).ToArray();
            //var term = (from user in _context.Users
            //            where user.LastName.StartsWith(prefix) &&
            //                (!userIds.Contains(user.UserId) && user.AcademyId == AcademyId && !users.Contains(user.UserId) && user.IsActive == true
            //                && user.IsDelete == false)
            //            select new
            //            {
            //                label = user.Gender.GenderName + " " + user.FirstName + " " + user.LastName,
            //                val = user.UserId
            //            }).ToList().Take(3);
            //return Json(term);
            var userIds = _context.userCourses.Where(uc => uc.CourseId == CourseId).Select(uc => uc.UserId).ToArray();
            var term = (from user in _context.Users
                        where user.LastName.StartsWith(prefix) &&
                            (!userIds.Contains(user.UserId) && user.AcademyId == AcademyId && user.IsActive == true)
                        select new
                        {
                            label = user.Gender.GenderName + " " + user.FirstName + " " + user.LastName,
                            val = user.UserId
                        }).ToList().Take(10);
            return Json(term);
        }

        [HttpPost]
        public JsonResult SupportTeachersAutoComplete(string prefix, int StateId, int AcademyId)
        {
            var cities = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            var TeacherIds = _context.TeacherAcademies.Where(ta => ta.AcademyId == AcademyId).Select(ta => ta.TeacherId).ToArray();
            var term = (from NewTeacher in _context.NewTeachers
                        where NewTeacher.LastName.StartsWith(prefix) && cities.Contains(NewTeacher.CityCode) && !TeacherIds.Contains(NewTeacher.TeacherId)
                        select new
                        {
                            label = NewTeacher.Gender.GenderName + " " + NewTeacher.FirstName + " " + NewTeacher.LastName,
                            val = NewTeacher.TeacherId
                        }).ToList();
            return Json(term);
        }

        [HttpPost]
        public JsonResult SupportAutoComplete(string prefix , int StateId)
        {
            var cities = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            var term = (from Academy in _context.Academies
                        where Academy.AcademyName.StartsWith(prefix) && cities.Contains(Academy.CityId)
                        select new
                        {
                            label = Academy.AcademyFullName,
                            val = Academy.AcademyId
                        }).ToList();
            return Json(term);
        }

        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
            ViewBag.GenderList = genders;
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            //register.AcademyId = Convert.ToInt16(Request.Form.Keys.Equals("AcademyId"));
            if (register.AcademyId == 0)
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
                ViewBag.GenderList = genders;
                ModelState.AddModelError("AcademyId", "آموزشگاه وارد شده معتبر نمی باشد");
                return View(register);
            }
            
            if (!ModelState.IsValid)
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
                ViewBag.GenderList = genders;
                return View(register);
            }
            if(register.Mobile.Length < 11)
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
                ViewBag.GenderList = genders;
                ModelState.AddModelError("Mobile", "شماره موبایل به صورت صحیح وارد نشده است");
                return View(register);
            }
            if (_userService.IsExsitMobile(FixedText.FixedMobile(register.Mobile)))
            {
                List<Gender> genders = new List<Gender>();
                genders = _userService.Genders();
                genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
                ViewBag.GenderList = genders;
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده تکراری است");
                return View(register);
            }

            Random r = new Random();
            //string ActiveCode = string.Empty;
            DataLayer.Entities.User.User user = new User()
            {
                GenderId = register.GenderId,
                AcademyId = register.AcademyId,
                Mobile = FixedText.FixedMobile(register.Mobile),
                LastName = FixedText.FixedTxt(register.LastName),
                Password = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(register.Password)),
                FirstName = FixedText.FixedTxt(register.FirstName),
                ActiveCode = r.Next().ToString().Substring(0, 4),
                CreateDate = DateTime.Now,
                IsActive = false,
                UserAvatar = "default.png",
            };

            _userService.AddUser(user);

            #region Send Activation SMS
            try
            {
                SendSms.SendSMS(FixedText.FixedMobile(register.Mobile), user.ActiveCode);
            }
            catch
            {
                return View("SuccessRegister", model: user);
            }

            #endregion

            return View("SuccessRegister",model:user);
        }

        [Route("RegisterTeacher")]
        public IActionResult RegisterTeacher()
        {
            List<Gender> genders = new List<Gender>();
            genders = _userService.Genders();
            genders.Insert(0, new Gender { GenderId = 0, GenderName = "---- انتخاب ----" });
            ViewBag.GenderList = genders;
            return View();
        }

        [Route("RegisterTeacher")]
        [HttpPost]
        public IActionResult RegisterTeacher(RegisterTeacherViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExsitTeacherMobile(FixedText.FixedMobile(register.Mobile)))
            {
                ModelState.AddModelError("Mobile", "شماره موبایل وارد شده تکراری است");
                return View(register);
            }

            Random r = new Random();
            //string ActiveCode = string.Empty;
            NewTeacher teacher = new NewTeacher()
            {
                GenderId = register.GenderId,
                CityCode = register.CityCode,
                Mobile = FixedText.FixedMobile(register.Mobile),
                LastName = FixedText.FixedTxt(register.LastName),
                Password = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(register.Password)),
                FirstName = FixedText.FixedTxt(register.FirstName),
                ActiveCode = r.Next().ToString().Substring(0, 4),
                CreateDate = DateTime.Now,
                IsActive = false,
                UserAvatar = "default.png",
            };
            _userService.AddTeacher(teacher);
            return View("SuccessTeacherRegister", model: teacher);
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login(bool EditProfile = false, bool permission = true)
        {
            ViewBag.EditProfile = EditProfile;
            ViewBag.Permission = permission;
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            if (!Captcha.ValidateCaptchaCode(FixedText.FixedEmail(login.CaptchaCode), HttpContext))
            {
                ModelState.AddModelError("CaptchaCode", "کد وارد شده صحیح نمی باشد .");
            }
            else

            {
                login.Mobile = FixedText.FixedMobile(login.Mobile);
                login.Password = FixedText.FixedTxt(login.Password);
                login.CaptchaCode = FixedText.FixedEmail(login.CaptchaCode);
                var user = _userService.LoginUser(login);
                if (user != null)
                {
                    var academy = _userService.GetAcademy(user.AcademyId);
                    var gender = _userService.GetGender(user.GenderId);
                    if (user.IsActive)
                    {
                        var claim = new List<Claim>() // مرحله 3 کلایم
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.FirstName + " " + user.LastName),
                        new Claim(ClaimTypes.MobilePhone,user.Mobile),
                        new Claim(ClaimTypes.Gender,gender.GenderName),
                        new Claim(ClaimTypes.Surname,academy.AcademyName),
                        new Claim(ClaimTypes.UserData,user.UserAvatar),
                        new Claim(ClaimTypes.Actor,academy.AcademyId.ToString())
                    };
                        var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe // باید از ویو مدل استفاده کنیم چون ریممبر در لاگین است
                        };

                        HttpContext.SignInAsync(principal, properties); // آخرین مرحله کلایم
                        ViewBag.IsSuccess = true;
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال می باشد");
                    }
                }
                ModelState.AddModelError("Password", "نام کاربری یا کلمه عبور اشتباه می باشد");
            }
            return View();
        }

        [Route("LoginTeacher")]
        public IActionResult LoginTeacher(bool EditProfile = false, bool permission = true)
        {
            ViewBag.EditProfile = EditProfile;
            ViewBag.Permission = permission;
            return View();
        }

        [Route("LoginTeacher")]
        [HttpPost]
        public IActionResult LoginTeacher(LoginTeacherViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            if (!Captcha.ValidateCaptchaCode(FixedText.FixedEmail(login.CaptchaCode), HttpContext))
            {
                ModelState.AddModelError("CaptchaCode", "کد وارد شده صحیح نمی باشد .");
            }
            else

            {
                login.Mobile = FixedText.FixedMobile(login.Mobile);
                login.Password = FixedText.FixedTxt(login.Password);
                login.CaptchaCode = FixedText.FixedEmail(login.CaptchaCode);
                var user = _userService.LoginTeacher(login);
                if (user != null)
                {
                    var gender = _userService.GetGender(user.GenderId);
                    if (user.IsActive)
                    {
                        var claim = new List<Claim>() // مرحله 3 کلایم
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.TeacherId.ToString()),
                        new Claim(ClaimTypes.Name,user.FirstName + " " + user.LastName),
                        new Claim(ClaimTypes.MobilePhone,user.Mobile),
                        new Claim(ClaimTypes.Gender,gender.GenderName),
                        new Claim(ClaimTypes.UserData,user.UserAvatar),
                        new Claim(ClaimTypes.Actor,"0")
                    };
                        var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe // باید از ویو مدل استفاده کنیم چون ریممبر در لاگین است
                        };

                        HttpContext.SignInAsync(principal, properties); // آخرین مرحله کلایم
                        ViewBag.IsSuccess = true;
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال می باشد");
                    }
                }
                ModelState.AddModelError("Password", "نام کاربری یا کلمه عبور اشتباه می باشد");
            }
            return View();
        }

        #region LogOut
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        [Route("LogoutTeacher")]
        public IActionResult LogoutTeacher()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/LoginTeacher");
        }

        [Route("LogoutInspector")]
        public IActionResult LogoutInspector()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion
        #endregion

        #region Active Account

        [Route("ActivateAccount")]
        public IActionResult ActivateAccount(string id)
        {
            string ack = FixedText.FixedTxt(HttpContext.Request.Form["ActiveCode"].ToString());
            ViewBag.IsActive = _userService.ActiveAccount(id, ack);
            return View();
        }

        [Route("ActivateTeacherAccount")]
        public IActionResult ActivateTeacherAccount(string id)
        {
            return View();
        }


        #endregion

        [Route("ReSendActivateSMS")]
        public IActionResult ReSendActivateSMS()
        {
            return View("SuccessRegister");
        }

        [HttpPost]
        [Route("ReSendActivateSMS")]
        public IActionResult ReSendActivateSMS(ReSendActivateCodeViewModel reSend)
        {

            reSend.Mobile = FixedText.FixedMobile(HttpContext.Request.Query["id"]);
            User user = _userService.GetUserByMobile(reSend.Mobile);
            if(user == null)
            {
                ModelState.AddModelError("Mobile", "کاربر یافت نشد");
                return View(reSend);
            }

            #region Send Activation SMS
            try
            {
                SendSms.SendSMS(FixedText.FixedMobile(user.Mobile), FixedText.FixedTxt(user.ActiveCode));
            }
            catch
            {
                ViewBag.IsSuccess = true;
                return View("ReSendActivateSMS", model: user);
            }

            #endregion
            
            ViewBag.IsSuccess = true;
            return View("ReSendActivateSMS", model: user);
        }

        [Route("ReSendTeacherActivateSMS")]
        public IActionResult ReSendTeacherActivateSMS()
        {
            return View("SuccessTeacherRegister");
        }

        [HttpPost]
        [Route("ReSendTeacherActivateSMS")]
        public IActionResult ReSendTeacherActivateSMS(ReSendActivateCodeViewModel reSend)
        {

            reSend.Mobile = FixedText.FixedMobile(HttpContext.Request.Query["id"]);
            NewTeacher newTeacher = _userService.GetTeacherByMobile(reSend.Mobile);
            if (newTeacher == null)
            {
                ModelState.AddModelError("Mobile", "کاربر یافت نشد");
                return View(reSend);
            }
            //SendSms.Send(user.Mobile, user.ActiveCode);
            SendSms.SendSMS(FixedText.FixedMobile(newTeacher.Mobile), FixedText.FixedTxt(newTeacher.ActiveCode));
            ViewBag.IsSuccess = true;
            return View("ReSendTeacherActivateSMS", model: newTeacher);
        }



        public IActionResult SuccessRegister()
        {
            return View();
        }

        public IActionResult SuccessTeacherRegister()
        {
            return View();
        }
        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotTeacherPassword")]
        public IActionResult ForgotTeacherPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);
            string fixedMobile = FixedText.FixedMobile(forgot.Mobile);
            bool acountStatus = _userService.ForgotActivateCode(fixedMobile);
            DataLayer.Entities.User.User user = _userService.GetUserByMobile(fixedMobile);

            if (!Captcha.ValidateCaptchaCode(FixedText.FixedEmail(forgot.CaptchaCode), HttpContext))
            {
                ModelState.AddModelError("CaptchaCode", "کد وارد شده صحیح نمی باشد .");
                return View("_ForgotPassword", model: user);
            }
            
            else
            {
                if (user == null)
                {
                    ModelState.AddModelError("Mobile", "کاربری یافت نشد");
                    return View(forgot);
                }
                else if(user != null && acountStatus == false)
                {
                    ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال می باشد. لطفا با مدیر آموزشگاه تماس بگیرد");
                    return View(forgot);
                }

                SendSms.SendSMS(FixedText.FixedMobile(user.Mobile), user.ActiveCode);
                ViewBag.IsSuccess = true;
                //return View();
            }
            
            return View("_ForgotPassword", model: user);
        }

        [HttpPost]
        [Route("ForgotTeacherPassword")]
        public IActionResult ForgotTeacherPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string fixedMobile = FixedText.FixedMobile(forgot.Mobile);
            bool acountStatus = _userService.ForgotTeacherActivateCode(fixedMobile);
            
            NewTeacher user = _userService.GetTeacherByMobile(fixedMobile);

            if (!Captcha.ValidateCaptchaCode(FixedText.FixedEmail(forgot.CaptchaCode), HttpContext))
            {
                ModelState.AddModelError("CaptchaCode", "کد وارد شده صحیح نمی باشد .");
                return View("_ForgotTeacherPassword", model: user);
            }

            else
            {
                if (user == null)
                {
                    ModelState.AddModelError("Mobile", "کاربری یافت نشد");
                    return View(forgot);
                }
                else if (user != null && acountStatus == false)
                {
                    ModelState.AddModelError("Mobile", "حساب کاربری شما غیر فعال می باشد. لطفا با مدیر سامانه تماس بگیرد");
                    return View(forgot);
                }
                SendSms.SendSMS(FixedText.FixedMobile(user.Mobile), FixedText.FixedTxt(user.ActiveCode));
                ViewBag.IsSuccess = true;
                //return View();
            }

            return View("_ForgotTeacherPassword", model: user);
        }
        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string Mobile)
        {
            //Mobile = HttpContext.Request.Query["Mobile"]; //HttpContext.Request.Form["ActiveCode"].ToString();
            ResetPasswordViewModel rst = new ResetPasswordViewModel();
            rst.Mobile = HttpContext.Request.Query["Mobile"];
            rst.ActiveCode = FixedText.FixedTxt(HttpContext.Request.Query["ActiveCode"]);
            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(rst.Mobile, rst.ActiveCode);
            if (user == null)
                return View("ForgotPassword");
            return View(rst);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View("ForgotPassword");
            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(reset.Mobile, FixedText.FixedTxt(reset.ActiveCode));
            string hashNewPassword = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(reset.Password));
            user.Password = hashNewPassword;
            user.ActiveCode = null;
            _userService.UpdateUser(user);
            return Redirect("/Login");
        }

        public IActionResult TeacherResetPassword(string Mobile)
        {
            //id = HttpContext.Request.Query["ActiveCode"]; //HttpContext.Request.Form["ActiveCode"].ToString();
            ResetPasswordViewModel rst = new ResetPasswordViewModel();
            rst.Mobile = HttpContext.Request.Query["Mobile"];
            rst.ActiveCode = FixedText.FixedTxt(HttpContext.Request.Query["ActiveCode"]);
            NewTeacher newTeacher = _userService.GetTeacherByActiveCode(rst.Mobile, rst.ActiveCode);
            if (newTeacher == null)
                return View("ForgotTeacherPassword");
            return View(rst);
        }

        [HttpPost]
        public IActionResult TeacherResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View("ForgotTeacherPassword");
            NewTeacher newTeacher = _userService.GetTeacherByActiveCode(reset.Mobile, FixedText.FixedTxt(reset.ActiveCode));
            string hashNewPassword = PasswordHelper.EncodePasswordMd5(FixedText.FixedTxt(reset.Password));
            newTeacher.Password = hashNewPassword;
            newTeacher.ActiveCode = null;
            _userService.UpdateTeacher(newTeacher);
            return Redirect("/LoginTeacher");
        }

        #endregion
    }
}