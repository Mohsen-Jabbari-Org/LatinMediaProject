using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class ValidTimesCourse
    {
        [Key]
        public int ValidTimesCourseId { get; set; }

        [Display(Name = "ساعت مجاز کلاس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ValidTimesCourseName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool EvenOdd { get; set; }

        #region Relations
        public List<ValidTimesAcademyList> ValidTimesAcademyLists { get; set; }
        #endregion
    }
}
