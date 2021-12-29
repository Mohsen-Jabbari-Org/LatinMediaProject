using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.Core.ViewModels
{
  public class InformationUserViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Academy { get; set; }
        public string Mobile { get; set; }
        public string UserAvatar { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
        public string GenderName { get; set; }
        public string AcademyName { get; set; }
        public int AcademyId { get; set; }

    }

    public class InformationSupporterViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Province { get; set; }
        public string Mobile { get; set; }
        public string UserAvatar { get; set; }
        public DateTime RegisterDate { get; set; }
        public string GenderName { get; set; }

    }

    public class InformationTeacherViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Academy { get; set; }
        public string Mobile { get; set; }
        public string CityName { get; set; }
        public string UserAvatar { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
        public string GenderName { get; set; }

    }

    public class InformationInspectorViewModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Academy { get; set; }
        public string Mobile { get; set; }
        public string StateName { get; set; }
        public string UserAvatar { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Wallet { get; set; }
        public string GenderName { get; set; }

    }

    #region EditProfileViewModel
    public class EditProfileViewModel
    {
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LastName { get; set; }

        [Display(Name = "آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int AcademyId { get; set; }


        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

        public string Mobile { get; set; }
    }

    public class EditSupportProfileViewModel
    {
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LastName { get; set; }

        [Display(Name = "استان")]
        public string Province { get; set; }


        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

        public string Mobile { get; set; }
    }

    public class EditTeacherProfileViewModel
    {
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LastName { get; set; }

        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

        public string Mobile { get; set; }
    }

    public class EditInspectorProfileViewModel
    {
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LastName { get; set; }

        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

        public string Mobile { get; set; }
    }
    #endregion

    #region EditProfileAdminViewModel
    public class EditProfileAdminViewModel
    {
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int GenderId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string LastName { get; set; }

        public int AcademyId { get; set; }

        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }

        public string Mobile { get; set; }
    }
    #endregion


    public class ChangePasswordViewModel
    {

        [Display(Name = "کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string OldPassword { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Compare("Password", ErrorMessage = "کلمه عبور مغایرت دارد")]
        public string RePassword { get; set; }
    }

    public class ChangeMobileViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Mobile { get; set; }

        [Display(Name = "تکرار شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [Compare("Mobile", ErrorMessage = "شماره موبایل های وارد شده یکسان نیست")]
        public string ReMobile { get; set; }

        public string Token { get; set; }

    }
}
