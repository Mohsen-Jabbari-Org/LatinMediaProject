using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Survay
{
    public class Education
    {
        [Key]
        public int EduId { get; set; }

        [Display(Name = "عنوان تحصیلات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string EduName { get; set; }

        #region Relations
        public List<Survay> Survays { get; set; }
        #endregion
    }
}
