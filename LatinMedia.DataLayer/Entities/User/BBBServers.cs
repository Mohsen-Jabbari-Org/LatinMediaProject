using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class BBBServers
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام سرور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ServerName { get; set; }

        [Display(Name = "آدرس سرور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ServerUrl { get; set; }

        [Display(Name = "توکن سرور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ServerSharesSecret { get; set; }

        public bool IsActive { get; set; }

        #region Relation
        public List<Academy> Academies { get; set; }
        public List<Course.Course> Courses { get; set; }
        #endregion
    }
}
