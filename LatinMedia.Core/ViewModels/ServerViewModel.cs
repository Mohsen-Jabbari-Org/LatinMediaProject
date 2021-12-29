using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.ViewModels
{
    public class ServerViewModel
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ServerUrl { get; set; }
        public string ServerSharesSecret { get; set; }
        public int ServerNullMeetings { get; set; }
        public int ServerInUseMeetings { get; set; }
        public bool IsActive { get; set; }
    }
}
