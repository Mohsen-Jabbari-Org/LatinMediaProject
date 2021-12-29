using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Survay
{
    public class Survay
    {
        [Key]
        public int SurvayId { get; set; }

        [Display(Name = "شناسه کاربر")]
        public int UserId { get; set; }

        [Display(Name = "جنسیت")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "شماره تماس")]
        public string Mobile { get; set; }

        [Display(Name = "سن")]
        public string Age { get; set; }

        [Display(Name = "شغل")]
        public int EmpId { get; set; }

        [Display(Name = "تحصیلات")]
        public int EduId { get; set; }

        public int Poll1 { get; set; }
        public int Poll2 { get; set; }
        public int Poll3 { get; set; }
        public int Poll4 { get; set; }
        public int Poll5 { get; set; }
        public string Comment { get; set; }
        public DateTime SurvayDate { get; set; }

        #region Relations
        public Employment Employment { get; set; }
        public Education Education { get; set; }
        #endregion
    }
}
