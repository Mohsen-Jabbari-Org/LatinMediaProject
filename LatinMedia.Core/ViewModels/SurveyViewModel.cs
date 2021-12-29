using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LatinMedia.Core.ViewModels
{
    public class SurveyViewModel
    {
        public int SurvayId { get; set; }
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
        public int Employment { get; set; }

        [Display(Name = "تحصیلات")]
        public int Education { get; set; }

        public int Poll1 { get; set; }
        public int Poll2 { get; set; }
        public int Poll3 { get; set; }
        public int Poll4 { get; set; }
        public int Poll5 { get; set; }
        public int Poll6 { get; set; }
        public int Poll7 { get; set; }
        public int Poll8 { get; set; }
        public int Poll9 { get; set; }
        public int Poll10 { get; set; }
        public int Poll11 { get; set; }
        public int Poll12 { get; set; }
        public int Poll13 { get; set; }
        public int Poll14 { get; set; }
        public int Poll15 { get; set; }
        public int Poll16 { get; set; }
        public int Poll17 { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public bool IsPolled { get; set; }
        public List<TeachersForSurvay> teachersForSurvays { get; set; }
    }

    public class SortedClassViewModel
    {
        public int Counter { get; set; }
        public int CourseId { get; set; }
        public DateTime ClassDate { get; set; }
    }

    public class TeachersForSurvay
    {
        public int TeacherId { get; set; }
        public int TeacherType { get; set; }
        public string TeacherFullName { get; set; }
        public string TeacherTypeName { get; set; }
    }

    public class PagingForSurvayDetailViewModel
    {
        public int UserCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int LastPage { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }
    }
}
