using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;

namespace LatinMedia.Core.ViewModels
{
    public class UsersForAdminViewModel
    {
        public List<User> Users { get; set; }

        public int UserCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class TestUsersForAdminViewModel
    {
        public List<User> Users { get; set; }
        public int UserCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int LastPage { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }
    }

    public class ServersForAdminViewModel
    {
        public List<BBBServers> BBBServers { get; set; }
        public int ServerCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class TeachersForAdminViewModel
    {
        public List<NewTeacher> NewTeachers { get; set; }
        public int TeacherCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class AcademiesForAdminViewModel
    {
        public List<Academy> Academies { get; set; }
        public int AcademyCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class StatesForAdminViewModel
    {
        public List<State> States { get; set; }
        public int StatesCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class CitiesForAdminViewModel
    {
        public List<City> Cities { get; set; }
        public int CityCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class CourseListForAdminViewModel
    {
        public List<Course> Courses { get; set; }
        public int CourseCounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class ReportForAdminViewModel
    {
        public int UserCounts { get; set; }
        public int TeacherCounts { get; set; }
        public int FinishedClass { get; set; }
        public int CurruntClass { get; set; }
    }

    public class ReportForSurvayViewModel
    {
        public int StateId { get; set; }
        public int GenderId { get; set; }
        public int EduId { get; set; }
        public int EmpId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ReportForTeacherViewModel
    {
        public int TeacherId { get; set; }
        public int GenderId { get; set; }
        public int EduId { get; set; }
        public int EmpId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ReportForSurvayDetailViewModel
    {
        public int TeacherId { get; set; }
        public string TeacherFullName { get; set; }
        public double Poll1_1 { get; set; }
        public double Poll1_2 { get; set; }
        public double Poll1_3 { get; set; }
        public double Poll1_4 { get; set; }
        public double Poll2_1 { get; set; }
        public double Poll2_2 { get; set; }
        public double Poll2_3 { get; set; }
        public double Poll2_4 { get; set; }
        public double Poll3_1 { get; set; }
        public double Poll3_2 { get; set; }
        public double Poll3_3 { get; set; }
        public double Poll3_4 { get; set; }
        public double Poll4_1 { get; set; }
        public double Poll4_2 { get; set; }
        public double Poll4_3 { get; set; }
        public double Poll4_4 { get; set; }
        public double Poll5_1 { get; set; }
        public double Poll5_2 { get; set; }
        public double Poll5_3 { get; set; }
        public double Poll5_4 { get; set; }
        public double Poll6_1 { get; set; }
        public double Poll6_2 { get; set; }
        public double Poll6_3 { get; set; }
        public double Poll6_4 { get; set; }
        public int UserCount { get; set; }
    }

    public class ReportForSurvayDetailTeachersViewModel
    {
        public int AcademyId { get; set; }
        public string AcademyFullName { get; set; }
        public double Poll1_1 { get; set; }
        public double Poll1_2 { get; set; }
        public double Poll1_3 { get; set; }
        public double Poll1_4 { get; set; }
        public double Poll2_1 { get; set; }
        public double Poll2_2 { get; set; }
        public double Poll2_3 { get; set; }
        public double Poll2_4 { get; set; }
        public double Poll3_1 { get; set; }
        public double Poll3_2 { get; set; }
        public double Poll3_3 { get; set; }
        public double Poll3_4 { get; set; }
        public double Poll4_1 { get; set; }
        public double Poll4_2 { get; set; }
        public double Poll4_3 { get; set; }
        public double Poll4_4 { get; set; }
        public double Poll5_1 { get; set; }
        public double Poll5_2 { get; set; }
        public double Poll5_3 { get; set; }
        public double Poll5_4 { get; set; }
        public double Poll6_1 { get; set; }
        public double Poll6_2 { get; set; }
        public double Poll6_3 { get; set; }
        public double Poll6_4 { get; set; }
        public int UserCount { get; set; }
    }

    public class ReportForSurvayCommentsTeachersViewModel
    {
        public int SurvayId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Age { get; set; }
        public string AcademyFullName { get; set; }
        public string Comment { get; set; }
    }
    public class CreateAcademyViewModel
    {
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

        public int BBBServerId { get; set; }

    }

    public class EditAcademyViewModel
    {
        [Display(Name = "نام آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyName { get; set; }

        public int CityCode { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(75, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CityName { get; set; }

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
        public List<int> ValidTimesCourse { get; set; }
        public List<int> AcademyType { get; set; }
        public int BBBServerId { get; set; }

    }
    public class CreateUserViewModel
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

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Mobile { get; set; }

        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }

    }

    public class CreateTeacherViewModel
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

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string Mobile { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CityCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }

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

        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        public int oldAcademyId { get; set; }
        public int AcademyId { get; set; }

        [Display(Name = "آموزشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(75, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string AcademyName { get; set; }

        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }

        public string AvatarName { get; set; }

        public List<int> UserRoles { get; set; }


    }

    public class EditTeacherViewModel
    {
        public int TeacherId { get; set; }

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

        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]

        public string Password { get; set; }

        public int CityCode { get; set; }

        [Display(Name = "محل خدمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(75, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CityName { get; set; }

        public bool IsActive { get; set; }

        public IFormFile UserAvatar { get; set; }

        public string AvatarName { get; set; }

        public List<int> TeacherGroups { get; set; }

    }

    public class BBBServerEditViewModel
    {
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
    }

    public class CreateCityViewModel
    {

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CityName { get; set; }

        public int StateId { get; set; }

        public int MaxUsers { get; set; }
    }

    public class CityEditViewModel
    {
        public int CityId { get; set; }

        [Display(Name = "نام شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد.")]
        public string CityName { get; set; }

        public int StateId { get; set; }

        public int MaxUsers { get; set; }
    }
}
