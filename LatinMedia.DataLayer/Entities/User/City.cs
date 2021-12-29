using LatinMedia.DataLayer.Entities.Teacher;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CityName { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public int StateId { get; set; }

        public int MaxUsers { get; set; }

        #region Relation
        public State State { get; set; }
        public List<Academy> Academies { get; set; }
        public List<NewTeacher> NewTeachers { get; set; }
        #endregion
    }
}
