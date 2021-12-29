using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Survay
{
    public class FourItems
    {
        [Key]
        public int ItemId { get; set; }

        [Display(Name = "عنوان گزینه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string ItemName { get; set; }
    }
}
