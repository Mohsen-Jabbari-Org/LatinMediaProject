using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class State
    {
        [Key]
        public int StateId { get; set; }

        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string StateName { get; set; }

        #region Relation
        public List<City> Cities { get; set; }

        public List<SupportTell> SupportTells { get; set; }
        #endregion
    }
}
