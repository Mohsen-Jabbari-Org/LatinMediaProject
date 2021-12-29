using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LatinMedia.DataLayer.Entities.Company;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Order;
using LatinMedia.DataLayer.Entities.Permissions;
using LatinMedia.DataLayer.Entities.Survay;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;

namespace LatinMedia.DataLayer.Context
{
    public class LatinMediaContext : DbContext
    {
        public LatinMediaContext(DbContextOptions<LatinMediaContext> options) : base(options)
        {
            
        }


        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Academy> Academies { get; set; }
        public DbSet<AcademyType> AcademyTypes { get; set; }
        public DbSet<AcademyTypeList> AcademyTypeLists { get; set; }
        public DbSet<ValidTimesCourse> ValidTimesCourses { get; set; }
        public DbSet<ValidTimesAcademyList> ValidTimesAcademyLists { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<UserDiscountCode> userDiscountCodes { get; set; }
        public DbSet<BBBServers> BBBServers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<GeneralEvents> GeneralEvents { get; set; }
        public DbSet<SupportTell> SupportTells { get; set; }

        #endregion

        #region Survay
        public DbSet<Employment> Employments { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Survay> Survays { get; set; }
        public DbSet<SurvayDetail> SurvayDetails { get; set; }
        public DbSet<TwoItems> TwoItems { get; set; }
        public DbSet<FourItems> FourItems { get; set; }
        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletTypes { get; set; }

        #endregion

        #region Permission

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region Course

        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> userCourses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }

        #endregion

        #region Teachers

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<NewTeacher> NewTeachers { get; set; }
        public DbSet<TeacherAcademy> TeacherAcademies { get; set; }
        public DbSet<TeacherGroup> TeacherGroups { get; set; }

        #endregion

        #region Company

        public DbSet<Company> Companies { get; set; }


        #endregion

        #region Order

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        #endregion

        #region SP

        public DbSet<ReportForSurvayDetailViewModel> reportForSurvays { get; set; }

        public DbSet<ReportForSurvayDetailTeachersViewModel> reportForSurvayTeachers { get; set; }
        public DbSet<AcademiesTeacher> GetAcademiesTeacherForSurvay { get; set; }
        public DbSet<CommentsTeacher> GetCommentsTeacherForSurvay { get; set; }

        public IQueryable<ReportForSurvayDetailViewModel> PollReport(int StateId, string StartDate, string EndDate , int GenderId , int EmpId , int EduId)
        {
            SqlParameter pStateId = new SqlParameter("@StateId", StateId);
            SqlParameter pStartDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter pEndDate = new SqlParameter("@EndDate", EndDate);
            SqlParameter pGenderId = new SqlParameter("@GenderId", GenderId);
            SqlParameter pEmpId = new SqlParameter("@EmpId", EmpId);
            SqlParameter pEduId = new SqlParameter("@EduId", EduId);
            return this.reportForSurvays.FromSql("EXECUTE spPoll @StateId , @StartDate , @EndDate , @GenderId , @EmpId , @EduId", pStateId, pStartDate, pEndDate, pGenderId, pEmpId, pEduId);
        }
        public IQueryable<ReportForSurvayDetailTeachersViewModel> PollTeacherReport(int TeacherId, int AcademyId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId)
        {
            SqlParameter pTeacherId = new SqlParameter("@TeacherId", TeacherId);
            SqlParameter pAcademyId = new SqlParameter("@AcademyId", AcademyId);
            SqlParameter pStartDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter pEndDate = new SqlParameter("@EndDate", EndDate);
            SqlParameter pGenderId = new SqlParameter("@GenderId", GenderId);
            SqlParameter pEmpId = new SqlParameter("@EmpId", EmpId);
            SqlParameter pEduId = new SqlParameter("@EduId", EduId);
            return this.reportForSurvayTeachers.FromSql("EXECUTE spPollTeacher @TeacherId , @AcademyId , @StartDate , @EndDate , @GenderId , @EmpId , @EduId", pTeacherId, pAcademyId , pStartDate, pEndDate, pGenderId, pEmpId, pEduId);
        }

        public IQueryable<AcademiesTeacher> GetTeachersAcademy(int TeacherId, string StartDate, string EndDate)
        {
            SqlParameter pTeacherId = new SqlParameter("@TeacherId", TeacherId);
            SqlParameter pStartDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter pEndDate = new SqlParameter("@EndDate", EndDate);
            return this.GetAcademiesTeacherForSurvay.FromSql("EXECUTE spGetTeachersAcademy @TeacherId , @StartDate , @EndDate", pTeacherId, pStartDate, pEndDate);
        }

        public IQueryable<CommentsTeacher> GetCommentTeacher(int TeacherId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId)
        {
            SqlParameter pTeacherId = new SqlParameter("@TeacherId", TeacherId);
            SqlParameter pStartDate = new SqlParameter("@StartDate", StartDate);
            SqlParameter pEndDate = new SqlParameter("@EndDate", EndDate);
            SqlParameter pGenderId = new SqlParameter("@GenderId", GenderId);
            SqlParameter pEmpId = new SqlParameter("@EmpId", EmpId);
            SqlParameter pEduId = new SqlParameter("@EduId", EduId);
            return this.GetCommentsTeacherForSurvay.FromSql("EXECUTE spPollCommentTeacher @TeacherId , @StartDate , @EndDate, @GenderId , @EmpId , @EduId", pTeacherId, pStartDate, pEndDate, pGenderId, pEmpId, pEduId);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete); // IsDelete = false
            modelBuilder.Entity<Role>()
                .HasQueryFilter(r => !r.IsDelete); // IsDelete = false
            modelBuilder.Entity<CourseGroup>()
               .HasQueryFilter(g => !g.IsDelete);
            modelBuilder.Entity<SurvayDetail>()
                .HasKey(nameof(SurvayDetail.SurvayId), nameof(SurvayDetail.TeacherId));
            base.OnModelCreating(modelBuilder);
        }
    }
}
