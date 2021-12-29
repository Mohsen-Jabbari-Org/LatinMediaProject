using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages.Leon.Servers
{
    [UserRoleChecker]
    [PermissionChecker(45)]
    public class IndexModel : PageModel
    {
        private IUserService _userService;
        BigBlueButtonAPISettings BigBlueButtonAPISettings = new BigBlueButtonAPISettings();
        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        private BigBlueButtonAPIClient client;
        private ICourseService _courseService;
        private IOrderService _orderService;
        public BBBServers BBBServers { get; set; }

        public IndexModel(BigBlueButtonAPIClient client, ICourseService courseService, IUserService userService, IOrderService orderService)
        {
            BigBlueButtonAPISettings.ServerAPIUrl = "https://km2.btnazmoon.ir/bigbluebutton/api/";
            BigBlueButtonAPISettings.SharedSecret = "nOhG82ZwcmDQMXWlBnenjdtuFe9c9QVzOGAkzSR5s";
            UrlBuilder UrlBuilder = new UrlBuilder(BigBlueButtonAPISettings);
            this.client = new BigBlueButtonAPIClient(BigBlueButtonAPISettings, httpClient);
            //this.client = client;
            _courseService = courseService;
            _userService = userService;
            _orderService = orderService;
        }

        public List<BBBServers> BBBServersList { get; set; }
        public List<ServerViewModel> ServerViewModels = new List<ServerViewModel>();
        public void OnGet()
        {
            BBBServersList = _userService.GetServers();
            for (int i = 1; i <= BBBServersList.Count; i++)
            {
                BBBServers = _courseService.GetServerTotalParameters(i);
                BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
                BigBlueButtonAPISettings.SharedSecret = string.Empty;
                BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
                BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
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
                
                if (meetingsCount == 0)
                {
                    ServerViewModel model = new ServerViewModel();
                    model.Id = BBBServers.Id;
                    model.ServerName = BBBServers.ServerName;
                    model.ServerSharesSecret = BBBServers.ServerSharesSecret;
                    model.ServerUrl = BBBServers.ServerUrl;
                    model.ServerNullMeetings = 0;
                    model.ServerInUseMeetings = 0;
                    model.IsActive = BBBServers.IsActive;
                    ServerViewModels.Add(model);
                }
                else
                {
                    ServerViewModel model = new ServerViewModel();
                    int NullCounter = 0, InUseCounter = 0;
                    model.Id = BBBServers.Id;
                    model.ServerName = BBBServers.ServerName;
                    model.ServerSharesSecret = BBBServers.ServerSharesSecret;
                    model.ServerUrl = BBBServers.ServerUrl;
                    for(int j = 0; j < r.Result.meetings.Count; j++)
                    {
                        if (r.Result.meetings[j].running == false)
                            NullCounter++;
                        else
                            InUseCounter++;
                    }
                    model.ServerNullMeetings = NullCounter;
                    model.ServerInUseMeetings = InUseCounter;
                    model.IsActive = BBBServers.IsActive;
                    ServerViewModels.Add(model);
                }
            }
        }
    }
}