using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.ViewModels
{
  public  class SideBarAdminPanelViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarName { get; set; }

    }

    public class SideBarAcademyPanelViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarName { get; set; }
        public List<SupportTell> SupportTells { get; set; }

    }
}
