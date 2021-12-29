using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class AcademyType
    {
        [Key]
        public int AcademyTypeId { get; set; }

        [Display(Name = "گروه آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyTypeTitle { get; set; }

        [Display(Name = "نام گروه آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyTypeName { get; set; }
        public bool IsDelete { get; set; }

        #region Relations
        public List<AcademyTypeList> AcademyTypeLists { get; set; }
        #endregion
    }
}
