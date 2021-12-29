using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BigBlueButtonAPI.Core;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.Web.Pages
{
    public class PlayMovieModel : PageModel
    {
        private IUserService _userService;
        BigBlueButtonAPISettings BigBlueButtonAPISettings = new BigBlueButtonAPISettings();
        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        private BigBlueButtonAPIClient client;
        private ICourseService _courseService;
        private IOrderService _orderService;
        public BBBServers BBBServers { get; set; }
        public List<ClassMoviesViewModel> classMoviesViewModel = new List<ClassMoviesViewModel>();

        public PlayMovieModel(BigBlueButtonAPIClient client, ICourseService courseService, IUserService userService, IOrderService orderService)
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
        public void OnGet(string CI , int SI)
        {
            //List<ClassMoviesViewModel> classMoviesViewModel = new List<ClassMoviesViewModel>();
            BBBServers = _courseService.GetServerTotalParameters(SI);
            BigBlueButtonAPISettings.ServerAPIUrl = string.Empty;
            BigBlueButtonAPISettings.SharedSecret = string.Empty;
            BigBlueButtonAPISettings.ServerAPIUrl = BBBServers.ServerUrl;
            BigBlueButtonAPISettings.SharedSecret = BBBServers.ServerSharesSecret;
            var result = client.GetRecordingsAsync(new GetRecordingsRequest { meetingID = CI});
            int meetingsCount;
            try
            {
                meetingsCount = result.Result.recordings.Count;
            }
            catch
            {
                meetingsCount = 0;
            }
            //if(meetingsCount == 0)
            //    classMoviesViewModel.
            for (int i = 0; i < meetingsCount; i++)
            {
                ClassMoviesViewModel model = new ClassMoviesViewModel();
                model.meetingID = result.Result.recordings[i].meetingID;
                model.name = result.Result.recordings[i].name;
                model.recordID = result.Result.recordings[i].recordID;
                model.size = result.Result.recordings[i].size;
                model.Url = result.Result.recordings[i].playbacks[0].url;
                classMoviesViewModel.Add(model);
            }
        }
    }
}