using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class Academy
    {
        [Key]
        public int AcademyId { get; set; }

        [Display(Name = "نام آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyName { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CityId { get; set; }

        [Display(Name = "کد پسنی")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string PostalCode { get; set; }

        [Display(Name = "تلفن آموزشگاه")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyTell { get; set; }

        [Display(Name = "موسس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string OwnerName { get; set; }

        [Display(Name = "کد ملی موسس")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string OwnerId { get; set; }

        [Display(Name = "تلفن موسس")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string OwnerTell { get; set; }

        [Display(Name = "نام مدیر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ManagerName { get; set; }

        [Display(Name = "تلفن مدیر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ManagerTell { get; set; }

        [Display(Name = "کد ملی مدیر")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ManagerId { get; set; }

        [Display(Name = "آدرس آموزشگاه")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyAddress { get; set; }

        [Display(Name = "نام کامل آموزشگاه")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyFullName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }

        public bool IsDelete { get; set; }

        public int BBBServersId { get; set; }

        #region Relation
        public List<AcademyTypeList> AcademyTypeLists { get; set; }
        public List<ValidTimesAcademyList> ValidTimesAcademyLists { get; set; }
        public List<TeacherAcademy> TeacherAcademies { get; set; }
        public City City { get; set; }
        public BBBServers BBBServers { get; set; }
        #endregion
    }
}
