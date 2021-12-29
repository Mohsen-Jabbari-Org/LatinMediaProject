using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Survay
{
    public class Employment
    {
        [Key]
        public int EmpId { get; set; }

        [Display(Name = "عنوان شغل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string EmpName { get; set; }

        #region Relations
        public List<Survay> Survays { get; set; }
        #endregion
    }
}
