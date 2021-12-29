using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Models;
using LatinMedia.Core.Security;
using Microsoft.AspNetCore.HttpOverrides;

namespace LatinMedia.Web.Areas.UserPanel.Controllers
{
    [Authorize]
    public class ZoomController : Controller
    {
        public BBBServers BBBServers { get; set; }
        BigBlueButtonAPISettings BigBlueButtonAPISettings = new BigBlueButtonAPISettings();
        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        private BigBlueButtonAPIClient client;
        public List<BBBServers> BBBServerList { get; set; }
        private ICourseService _courseService;
        private IUserService _userService;
        private IOrderService _orderService;


        public ZoomController(BigBlueButtonAPIClient client, ICourseService courseService, IUserService userService, IOrderService orderService)
        {
            
            BigBlueButtonAPISettings.ServerAPIUrl = "https://km2.btnazmoon.ir/bigbluebutton/api/";
            BigBlueButtonAPISettings.SharedSecret = "nOhG82ZwcmDQMXWlBnenjdtuFe9c9QVzOGAkzSR5s";
            UrlBuilder UrlBuilder = new UrlBuilder(BigBlueButtonAPISettings);
            this.client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
            //this.client = client;
            _courseService = courseService;
            _userService = userService;
            _orderService = orderService;
            BBBServerList = _courseService.GetAllServers();
        }
        /// <summary>
        /// It ensures the settings of BigBlueButton is ok. 
        /// It just helps you run the demo normally. In product environment, this method is not needed.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> isBigBlueButtonAPISettingsOKAsync()
        {
            try
            {
                var res = await client.IsMeetingRunningAsync(new IsMeetingRunningRequest { meetingID = Guid.NewGuid().ToString() });
                if (res.returncode == Returncode.FAILED) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Start(string id)
        {
            if(id != "favicon.ico")
            {
                Course course = new Course();
                course = _courseService.GetCourseByMeetingId(id);
                if(course.ServerId != 0)
                {
                    BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                    BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                    BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                    var Oks = await isBigBlueButtonAPISettingsOKAsync();
                    if (Oks)
                    {
                        var result1 = await client.IsMeetingRunningAsync(new IsMeetingRunningRequest { meetingID = id });
                        if(result1.running == true)
                        {
                            var Id = course.DemoFileName;
                            var model = new StartModel
                            {
                                Id = Id,
                                Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                ServerId = BBBServers.Id,
                                ClassName = course.CourseFaTitle,
                                Duration = course.CourseTime,
                                MaxUsers = course.MaxUsers
                            };
                            return View(model);
                        }

                        else if(result1.running == false)
                        {
                            #region Load Balancing

                            int ServerCount = BBBServerList.Count;
                            List<BigBlueButtonAPIClient> clients = new List<BigBlueButtonAPIClient>();
                            List<string> results = new List<string>();
                            ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
                            GetMeetingsResponse result = new GetMeetingsResponse();

                            for (int i = 0; i < ServerCount; i++)
                            {
                                BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                                BigBlueButtonAPISettings.SharedSecret = string.Empty;
                                BigBlueButtonAPISettings.ServerAPIUrl = BBBServerList[i].ServerUrl;
                                BigBlueButtonAPISettings.SharedSecret = BBBServerList[i].ServerSharesSecret;
                                client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
                                Oks = await isBigBlueButtonAPISettingsOKAsync();
                                if (Oks)
                                {
                                    var r = client.GetMeetingsAsync();
                                    if (r.Result == null)
                                    {
                                        //میتینگ رو ایجاد کنه
                                        setServerForCourse.ClassId = id;
                                        setServerForCourse.ServerId = BBBServerList[i].Id;
                                        _orderService.SetServerForCourse(setServerForCourse);


                                        if (course.CompanyId != 0)
                                        {
                                            BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                            BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                            BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                            var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                            if (setupOk)
                                            {
                                                var Id = course.DemoFileName;
                                                var model = new StartModel
                                                {
                                                    Id = Id,
                                                    Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                    Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                    ServerId = BBBServers.Id,
                                                    ClassName = course.CourseFaTitle,
                                                    Duration = course.CourseTime,
                                                    MaxUsers = course.MaxUsers
                                                };
                                                return View(model);
                                            }
                                            return View();
                                        }
                                    }
                                    else
                                    {
                                        // لود بالانس باید انجام بشه
                                        int userReserveds = 0, maxUserss = 358;
                                        for (int x = 0; x < r.Result.meetings.Count; x++)
                                        {
                                            if (r.Result.meetings[x].running == true)
                                                userReserveds += r.Result.meetings[x].maxUsers.Value;
                                        }

                                        if (userReserveds <= maxUserss)
                                        {
                                            setServerForCourse.ClassId = id;
                                            setServerForCourse.ServerId = BBBServerList[i].Id;
                                            _orderService.SetServerForCourse(setServerForCourse);

                                            if (course.CompanyId != 0)
                                            {
                                                BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                                BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                                BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                                var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                                if (setupOk)
                                                {
                                                    var Id = course.DemoFileName;
                                                    var model = new StartModel
                                                    {
                                                        Id = Id,
                                                        Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                        Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                        ServerId = BBBServers.Id,
                                                        ClassName = course.CourseFaTitle,
                                                        Duration = course.CourseTime,
                                                        MaxUsers = course.MaxUsers
                                                    };
                                                    return View(model);
                                                }
                                                return View();
                                            }
                                        }
                                    }
                                }

                            }
                            #endregion
                        }

                        else
                        {
                            var rs = client.GetMeetingsAsync();
                            int userReserved = 0, maxUsers = 358;
                            for (int x = 0; x < rs.Result.meetings.Count; x++)
                            {
                                if (rs.Result.meetings[x].running == true)
                                    userReserved += rs.Result.meetings[x].maxUsers.Value;
                            }

                            if (userReserved <= maxUsers)
                            {
                                var Id = course.DemoFileName;
                                var model = new StartModel
                                {
                                    Id = Id,
                                    Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                    Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                    ServerId = BBBServers.Id,
                                    ClassName = course.CourseFaTitle,
                                    Duration = course.CourseTime,
                                    MaxUsers = course.MaxUsers
                                };
                                return View(model);
                            }

                            else
                            {
                                #region Load Balancing

                                int ServerCount = BBBServerList.Count;
                                List<BigBlueButtonAPIClient> clients = new List<BigBlueButtonAPIClient>();
                                List<string> results = new List<string>();
                                ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
                                GetMeetingsResponse result = new GetMeetingsResponse();

                                for (int i = 0; i < ServerCount; i++)
                                {
                                    BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                                    BigBlueButtonAPISettings.SharedSecret = string.Empty;
                                    BigBlueButtonAPISettings.ServerAPIUrl = BBBServerList[i].ServerUrl;
                                    BigBlueButtonAPISettings.SharedSecret = BBBServerList[i].ServerSharesSecret;
                                    client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
                                    Oks = await isBigBlueButtonAPISettingsOKAsync();
                                    if(Oks)
                                    {
                                        var r = client.GetMeetingsAsync();
                                        if (r.Result == null)
                                        {
                                            //میتینگ رو ایجاد کنه
                                            setServerForCourse.ClassId = id;
                                            setServerForCourse.ServerId = BBBServerList[i].Id;
                                            _orderService.SetServerForCourse(setServerForCourse);


                                            if (course.CompanyId != 0)
                                            {
                                                BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                                BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                                BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                                var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                                if (setupOk)
                                                {
                                                    var Id = course.DemoFileName;
                                                    var model = new StartModel
                                                    {
                                                        Id = Id,
                                                        Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                        Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                        ServerId = BBBServers.Id,
                                                        ClassName = course.CourseFaTitle,
                                                        Duration = course.CourseTime,
                                                        MaxUsers = course.MaxUsers
                                                    };
                                                    return View(model);
                                                }
                                                return View();
                                            }
                                        }
                                        else
                                        {
                                            // لود بالانس باید انجام بشه
                                            int userReserveds = 0, maxUserss = 358;
                                            for (int x = 0; x < r.Result.meetings.Count; x++)
                                            {
                                                if (r.Result.meetings[x].running == true)
                                                    userReserveds += r.Result.meetings[x].maxUsers.Value;
                                            }

                                            if (userReserveds <= maxUserss)
                                            {
                                                setServerForCourse.ClassId = id;
                                                setServerForCourse.ServerId = BBBServerList[i].Id;
                                                _orderService.SetServerForCourse(setServerForCourse);

                                                if (course.CompanyId != 0)
                                                {
                                                    BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                                    BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                                    BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                                    var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                                    if (setupOk)
                                                    {
                                                        var Id = course.DemoFileName;
                                                        var model = new StartModel
                                                        {
                                                            Id = Id,
                                                            Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                            Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                            ServerId = BBBServers.Id,
                                                            ClassName = course.CourseFaTitle,
                                                            Duration = course.CourseTime,
                                                            MaxUsers = course.MaxUsers
                                                        };
                                                        return View(model);
                                                    }
                                                    return View();
                                                }
                                            }
                                        }
                                    }

                                }
                                #endregion
                            }
                        }
                        return View();
                    }
                    else
                    {
                        #region Load Balancing

                        int ServerCount = BBBServerList.Count;
                        List<BigBlueButtonAPIClient> clients = new List<BigBlueButtonAPIClient>();
                        List<string> results = new List<string>();
                        ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
                        GetMeetingsResponse result = new GetMeetingsResponse();

                        for (int i = 0; i < ServerCount; i++)
                        {
                            BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                            BigBlueButtonAPISettings.SharedSecret = string.Empty;
                            BigBlueButtonAPISettings.ServerAPIUrl = BBBServerList[i].ServerUrl;
                            BigBlueButtonAPISettings.SharedSecret = BBBServerList[i].ServerSharesSecret;
                            client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
                            Oks = await isBigBlueButtonAPISettingsOKAsync();
                            if(Oks)
                            {
                                var r = client.GetMeetingsAsync();
                                if (r.Result == null)
                                {
                                    //میتینگ رو ایجاد کنه
                                    setServerForCourse.ClassId = id;
                                    setServerForCourse.ServerId = BBBServerList[i].Id;
                                    _orderService.SetServerForCourse(setServerForCourse);


                                    if (course.CompanyId != 0)
                                    {
                                        BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                        BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                        BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                        var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                        if (setupOk)
                                        {
                                            var Id = course.DemoFileName;
                                            var model = new StartModel
                                            {
                                                Id = Id,
                                                Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                ServerId = BBBServers.Id,
                                                ClassName = course.CourseFaTitle,
                                                Duration = course.CourseTime,
                                                MaxUsers = course.MaxUsers
                                            };
                                            return View(model);
                                        }
                                        return View();
                                    }
                                }
                                else
                                {
                                    // لود بالانس باید انجام بشه
                                    int userReserveds = 0, maxUserss = 358;
                                    for (int x = 0; x < r.Result.meetings.Count; x++)
                                    {
                                        if (r.Result.meetings[x].running == true)
                                            userReserveds += r.Result.meetings[x].maxUsers.Value;
                                    }

                                    if (userReserveds <= maxUserss)
                                    {
                                        setServerForCourse.ClassId = id;
                                        setServerForCourse.ServerId = BBBServerList[i].Id;
                                        _orderService.SetServerForCourse(setServerForCourse);

                                        if (course.CompanyId != 0)
                                        {
                                            BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                            BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                            BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                            var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                            if (setupOk)
                                            {
                                                var Id = course.DemoFileName;
                                                var model = new StartModel
                                                {
                                                    Id = Id,
                                                    Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                                    Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                                    ServerId = BBBServers.Id,
                                                    ClassName = course.CourseFaTitle,
                                                    Duration = course.CourseTime,
                                                    MaxUsers = course.MaxUsers
                                                };
                                                return View(model);
                                            }
                                            return View();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    return View();
                }
                
                else
                {
                    #region Load Balancing

                    int ServerCount = BBBServerList.Count;
                    List<BigBlueButtonAPIClient> clients = new List<BigBlueButtonAPIClient>();
                    List<string> results = new List<string>();
                    ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
                    GetMeetingsResponse result = new GetMeetingsResponse();

                    for (int i = 0; i < ServerCount; i++)
                    {
                        BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                        BigBlueButtonAPISettings.SharedSecret = string.Empty;
                        BigBlueButtonAPISettings.ServerAPIUrl = BBBServerList[i].ServerUrl;
                        BigBlueButtonAPISettings.SharedSecret = BBBServerList[i].ServerSharesSecret;
                        client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
                        var r = client.GetMeetingsAsync();
                        if (r.Result == null)
                        {
                            //میتینگ رو ایجاد کنه
                            setServerForCourse.ClassId = id;
                            setServerForCourse.ServerId = BBBServerList[i].Id;
                            _orderService.SetServerForCourse(setServerForCourse);


                            if (course.CompanyId != 0)
                            {
                                BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                if (setupOk)
                                {
                                    var Id = course.DemoFileName;
                                    var model = new StartModel
                                    {
                                        Id = Id,
                                        Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                        Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                        ServerId = BBBServers.Id,
                                        ClassName = course.CourseFaTitle,
                                        Duration = course.CourseTime,
                                        MaxUsers = course.MaxUsers
                                    };
                                    return View(model);
                                }
                                return View();
                            }
                        }
                        else
                        {
                            // لود بالانس باید انجام بشه
                            int userReserved = 0, maxUsers = 358;
                            for (int x = 0; x < r.Result.meetings.Count; x++)
                            {
                                if (r.Result.meetings[x].running == true)
                                    userReserved += r.Result.meetings[x].maxUsers.Value;
                            }
                            
                            if (userReserved <= maxUsers)
                            {
                                setServerForCourse.ClassId = id;
                                setServerForCourse.ServerId = BBBServerList[i].Id;
                                _orderService.SetServerForCourse(setServerForCourse);

                                if (course.CompanyId != 0)
                                {
                                    BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                                    BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                                    BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                                    var setupOk = await isBigBlueButtonAPISettingsOKAsync();
                                    if (setupOk)
                                    {
                                        var Id = course.DemoFileName;
                                        var model = new StartModel
                                        {
                                            Id = Id,
                                            Url = Url.Action("Join", "Zoom", new { Id = Id }, Request.Scheme),
                                            Name = User.Identity.GetGender() + " " + User.Identity.Name,
                                            ServerId = BBBServers.Id,
                                            ClassName = course.CourseFaTitle,
                                            Duration = course.CourseTime,
                                            MaxUsers = course.MaxUsers
                                        };
                                        return View(model);
                                    }
                                    return View();
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            return View();
        }

        /// <summary>
        /// Creates an meeting and join it.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Start(StartModel model)
        {
            BBBServers = _courseService.GetServerTotalParameters(model.ServerId);
            BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
            BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
            if (!ModelState.IsValid) return View(model);

            var result = await client.IsMeetingRunningAsync(new IsMeetingRunningRequest { meetingID = model.Id });
            if (result.running == true)
            {
                var response = await client.GetMeetingInfoAsync(new GetMeetingInfoRequest
                {
                    meetingID = model.Id,
                });

                var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
                {
                    meetingID = model.Id,
                    fullName = model.Name,
                    password = response.moderatorPW
                });
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _userService.AddEvent(Convert.ToInt32(User.Identity.GetID()), 2, DateTime.Now, remoteIpAddress.ToString(), model.Name + " با موفقیت وارد کلاس " + model.ClassName + " گردید ."
                    , _courseService.GetCourseByMeetingId(model.Id).CourseId, BBBServers.Id, model.Id);
                return Redirect(url);
            }
            else
            {
                var response = await client.GetMeetingInfoAsync(new GetMeetingInfoRequest
                {
                    meetingID = model.Id,
                });

                if(response.running == false)
                {
                    var endResult = await client.EndMeetingAsync(new EndMeetingRequest
                    {
                        meetingID = model.Id,
                        password = response.moderatorPW
                    });
                    if (result.returncode == Returncode.FAILED)
                        return View("Name", endResult);

                    //1. Create a meeting
                    var responseCreateMeeting = await client.CreateMeetingAsync(new CreateMeetingRequest
                    {
                        name = model.ClassName,
                        meetingID = model.Id,
                        webcamsOnlyForModerator = true,
                        maxParticipants = model.MaxUsers,
                        logoutURL = "https://lms.btnrahgosha.ir",
                        duration = model.Duration,
                        lockSettingsDisablePrivateChat = true,
                        lockSettingsDisableCam = true,
                        lockSettingsLockOnJoin = true,
                        lockSettingsDisableNote = true,
                        lockSettingsLockOnJoinConfigurable = true,
                        lockSettingsDisablePublicChat = true,
                        record = true,
                        autoStartRecording = true,
                        bannerText = model.ClassName,
                        copyright = "سامانه آموزش آنلاین موسسه راهگشا",
                        logo = "/images/logo.png"
                    });
                    //Check the response from the BigBlueButton server and return error if has error.
                    if (responseCreateMeeting.returncode == Returncode.FAILED)
                    {
                        //اگه میتینگ بده میاد اینجا. اینجا باید یه خاکی تو سرش بریزم
                        ConfigServersForCourses setServerForCourse = new ConfigServersForCourses();
                        int MaxServer = BBBServerList.Count;
                        if (model.ServerId == MaxServer)
                            model.ServerId = 2;
                        else
                            model.ServerId = model.ServerId + 1;
                        BBBServers = _courseService.GetServerTotalParameters(model.ServerId);
                        BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                        BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                        setServerForCourse.ClassId = model.Id;
                        setServerForCourse.ServerId = model.ServerId;
                        _orderService.SetServerForCourse(setServerForCourse);
                        ModelState.AddModelError("Name", "کاربر گرامی! سرعت اینترنت شما ضعیف می باشد. لطفا دوباره تلاش نمایید");
                        return View(model);
                    }

                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _userService.AddEvent(Convert.ToInt32(User.Identity.GetID()), 1, DateTime.Now, remoteIpAddress.ToString(), "کلاس " + model.ClassName + " با موفقیت توسط " + model.Name
                        + " ایجاد گردید .", _courseService.GetCourseByMeetingId(model.Id).CourseId, BBBServers.Id, model.Id);
                    //2. Join the meeting as moderator
                    var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
                    {
                        meetingID = model.Id,
                        fullName = model.Name,
                        password = responseCreateMeeting.moderatorPW
                    });
                    _userService.AddEvent(Convert.ToInt32(User.Identity.GetID()), 2, DateTime.Now, remoteIpAddress.ToString(), model.Name + " با موفقیت وارد کلاس " + model.ClassName + " گردید ."
                        , _courseService.GetCourseByMeetingId(model.Id).CourseId, BBBServers.Id, model.Id);
                    return Redirect(url);
                }
                else
                {
                    //1. Create a meeting
                    var responseCreateMeeting = await client.CreateMeetingAsync(new CreateMeetingRequest
                    {
                        name = model.ClassName,
                        meetingID = model.Id,
                        webcamsOnlyForModerator = true,
                        maxParticipants = model.MaxUsers,
                        logoutURL = "https://lms.btnrahgosha.ir",
                        duration = model.Duration,
                        lockSettingsDisablePrivateChat = true,
                        lockSettingsDisableCam = true,
                        lockSettingsLockOnJoin = true,
                        lockSettingsDisableNote = true,
                        lockSettingsLockOnJoinConfigurable = true,
                        lockSettingsDisablePublicChat = true,
                        record = true,
                        autoStartRecording = true,
                        bannerText = model.ClassName,
                        copyright = "سامانه آموزش آنلاین موسسه راهگشا",
                        logo = "/images/logo.png"
                    });
                    //Check the response from the BigBlueButton server and return error if has error.
                    if (responseCreateMeeting.returncode == Returncode.FAILED)
                    {
                        ModelState.AddModelError("Name", responseCreateMeeting.message);
                        return View(model);
                    }

                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    _userService.AddEvent(Convert.ToInt32(User.Identity.GetID()), 1, DateTime.Now, remoteIpAddress.ToString(), "کلاس " + model.ClassName + " با موفقیت توسط " + model.Name
                        + " ایجاد گردید .", _courseService.GetCourseByMeetingId(model.Id).CourseId, BBBServers.Id, model.Id);
                    //2. Join the meeting as moderator
                    var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
                    {
                        meetingID = model.Id,
                        fullName = model.Name,
                        password = responseCreateMeeting.moderatorPW
                    });
                    _userService.AddEvent(Convert.ToInt32(User.Identity.GetID()), 2, DateTime.Now, remoteIpAddress.ToString(), model.Name + " با موفقیت وارد کلاس " + model.ClassName + " گردید ."
                        , _courseService.GetCourseByMeetingId(model.Id).CourseId, BBBServers.Id, model.Id);
                    return Redirect(url);
                }
                
            }
            
        }

        [HttpGet]
        public ActionResult Join(string id) // وقتی کاربر روی لینک کلاس کلیک کرد این اکشن باید اجرا بشه
        {
            if (id != "favicon.ico")
            {
                User user = new User();
                Course course = new Course();
                user = _userService.GetUserByMobile(User.Identity.GetMobile());
                if (user.UserId != 0)
                {
                    user = _userService.GetUserByMobile(User.Identity.GetMobile());
                    course = _courseService.GetCourseByMeetingId(id);
                    BBBServers = _courseService.GetServerTotalParameters(course.ServerId);
                    BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                    BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
                    var model = new JoinModel
                    {
                        Id = id,
                        Name = User.Identity.GetGender() + " " + user.FirstName + " " + user.LastName,
                        ServerId = BBBServers.Id,
                        Mobile = user.Mobile,
                        ClassName = course.CourseFaTitle,
                    };
                    return View(model);
                }
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Join(JoinModel model)
        {
            BBBServers = _courseService.GetServerTotalParameters(model.ServerId);
            BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
            BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
            model.ServerId = BBBServers.Id;
            if (!ModelState.IsValid) return View(model);
            var response = await client.GetMeetingInfoAsync(new GetMeetingInfoRequest
            {
                meetingID = model.Id,
                
            });
            if (response.returncode == Returncode.FAILED)
            {
                ModelState.AddModelError("Name", "مدرس هنوز وارد کلاس نشده است. لطفا منتظر بمانید");
                return View(model);
            }
            if (response.running == false)
            {
                if (response.hasUserJoined == true)
                {
                    ModelState.AddModelError("Name", "زمان ورود به کلاس به اتمام رسیده است.");
                }
                else
                {
                    ModelState.AddModelError("Name", "کلاس هنوز شروع نشده است");
                }

                return View(model);
            }
            else
            {
                var attendeesList = response.attendees;
                for (int i = 0; i < attendeesList.Count; i++)
                {
                    if (attendeesList[i].userID == User.Identity.GetID())
                    {
                        ModelState.AddModelError("Name", "شما در کلاس حضور دارید. لطفا از حساب کاربری خود خارج شده و پس از یک دقیقه مجددا وارد شوید");
                        return View(model);
                    }
                }
                var url = client.GetJoinMeetingUrl(new JoinMeetingRequest
                {
                    meetingID = model.Id,
                    fullName = model.Name,
                    password = response.attendeePW,
                    userID = User.Identity.GetID()
                });
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _userService.AddUserEvent(Convert.ToInt32(User.Identity.GetID()), 2, DateTime.Now, remoteIpAddress.ToString(), 
                    model.Name + " با موفقیت وارد کلاس " + model.ClassName + " گردید .", _courseService.GetCourseByMeetingId(model.Id).CourseId);
                return Redirect(url);
            }
        }

    }
}