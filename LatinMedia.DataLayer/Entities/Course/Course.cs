using LatinMedia.DataLayer.Entities.Order;
using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Course
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public int? SubGroupId { get; set; }

        public int? SecondSubGroupId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public int ServerId { get; set; }

        [Display(Name = "عنوان لاتین آموزش")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseLatinTitle { get; set; }

        [Display(Name = "عنوان آموزش")]
        [MaxLength(400, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseFaTitle { get; set; }

        [Display(Name = "حجم اموزش")]
        public int Volume { get; set; }

        [Display(Name = "مدت زمان")]
        public int CourseTime { get; set; }

        public int VTA_Id { get; set; }

        [Display(Name = "زیر نویس")]
        public bool IsSubTitle { get; set; }

        [Display(Name = "زبان اموزشی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Language { get; set; }

        [Display(Name = "توضیحات کلاس ")]
        public string CourseDescription { get; set; }

        [Display(Name = "تاریخ انتشار")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ اتمام")]
        public DateTime EndDate { get; set; }

        [Display(Name = "قیمت آموزش")]
        public int CoursePrice { get; set; }

        [Display(Name = "تعداد هنرجویان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int MaxUsers { get; set; }

        [Display(Name = "فایل آموزش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CourseFileName { get; set; }

        [Display(Name = "تصویر آموزش")]
        [MaxLength(100)]
        public string CourseImageName { get; set; }

        [Display(Name = "دمو آموزش")]
        [MaxLength(500)]
        public string DemoFileName { get; set; }

        [Display(Name = "تعداد ویدئو")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CountFiles { get; set; }


        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }

        #region Relations

        [ForeignKey("GroupId")]
        public CourseGroup CourseGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public CourseGroup SubGroup { get; set; }

        [ForeignKey("SecondSubGroupId")]
        public CourseGroup SecondSubGroup { get; set; }
        public Academy Academy { get; set; }
        public BBBServers BBBServers { get; set; }
        public Teacher.Teacher Teacher { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<UserCourse> UserCourses { get; set; }
        public List<CourseComment> courseComments { get; set; }
        public List<Event> Events { get; set; }
        public List<UserEvent> UserEvents { get; set; }

        #endregion

    }
}
