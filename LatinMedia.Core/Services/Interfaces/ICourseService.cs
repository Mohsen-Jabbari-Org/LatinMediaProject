using System;
using System.Collections.Generic;
using System.Text;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Company;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.Core.Services.Interfaces
{
   public interface ICourseService
   {
        #region Groups
        List<CourseGroup> GetAllGroups();
        List<State> GetStates();
        List<SelectListItem> GetGroupsForManageCourse();
        List<SelectListItem> GetSubGroupsForManageCourse(int groupId);
        List<SelectListItem> GetSecondSubGroupsForManageCourse(int subGroupId);

        #endregion

        #region Teacher
        List<Teacher> GetAllTeachers();
        List<SelectListItem> GetTeachersForManageCourse();
        List<SelectListItem> GetTeachersFromUsersToManageCourse(int AcademyId);
        List<SelectListItem> GetTeachersFromCityToAddAcademy(int CityId, int AcademyId);
        List<SelectListItem> GetTeachersForSupportToAddAcademy(int StateId, int AcademyId);
        List<SelectListItem> GetTeachersFromUsersToManageCourse();
        List<SelectListItem> GetUsersFromUsersToManageCourse();
        List<SelectListItem> GetUsersFromUsersToManageCourse(int id , int companyId);

        List<ShowAllTeachersViewModel> ShowAllTeachers();

        #endregion

        #region Levels

        List<SelectListItem> GetLevelsForManageCourse();

        #endregion

        #region Company
        List<Company> GetAllCompanies();
        List<SelectListItem> GetCompaniesForManageCourse();
        List<SelectListItem> GetValidTimesForManageCourse(int academyId);
        //List<ShowAllCompaniesViewModel> ShowAllCompanies();
        #endregion

        #region Course Types

        List<SelectListItem> GetCourseTypesForManageCourse();
        List<CourseType> GetAllCourseTypes();
        string GetAcademyById(int AcademyId);

        #endregion

        #region Course
        List<ShowCourseForAdminViewModel> GetCoursesForAdmin();
        CourseListForAdminViewModel GetCoursesForAdmin(int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "");
        CourseListForAdminViewModel GetCoursesForSupport(int StateId, int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "");
        CourseListForAdminViewModel GetFinishedCoursesForAdmin(int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "");
        CourseListForAdminViewModel GetFinishedCoursesForSupport(int StateId, int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "");
        List<ShowCourseForAdminViewModel> GetCoursesForAcUsers(int AcademyId);
        List<ShowCourseForAdminViewModel> GetArchivedCoursesForAcUsers(int AcademyId);
        List<ShowCourseForAdminViewModel> GetCoursesForAcUsers();
        int AddCourse(Course course, IFormFile imgCourseUp, IFormFile courseFile);
        void DeleteCourse(int CourseId);
        Course GetCourseById(int courseId);
        Course GetCourseByMeetingId(string MeetingId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseFile);
        Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", int type = 0,
             int minPrice = 0, int maxPrice = 0, List<int> selectedGroups = null, int take = 0, int companyId = 0, int teacherId = 0);

        Course GetCourseForShow(int id);

        #endregion

        #region TeacherAcademy
        #endregion

        #region Comment

        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>, int> GetCourseComments(int courseId, int pageId = 1);

        #endregion

        void AddUserInClass(int userId, int courseId);
        void AddTeacherInAcademy(int TeacherId, int AcademyId);
        bool RemoveTeacherFromAcademy(int TeacherId, int AcademyId);
        void RemoveUserFromClass(int userCourseId, int courseId);
        List<UserCourse> GetUserCourse(int Cid);
        List<TeacherAcademy> GetAcademiesTeachers(int Cid);
        List<ShowUsersInClassViewModel> GetUsersInClass(int CourseId);
        List<BBBServers> GetAllServers();
        BBBServers GetServerParameters(int academyId);
        BBBServers GetServerTotalParameters(int serverId);
        string GetTimesForCourse(int VTA_Id);
        string GetGroupNameForAdmin(int GroupId);
        string GetTimesForArchivedCourse(int VTA_Id);
        string GetStartTimes(int VTA_Id);
        string GetEndTimes(int VTA_Id);
        List<SelectListItem> GetAcademiesForManageCourse();
        List<SelectListItem> GetAcademiesForSupportCourse(int stateId);

        bool IsUserInCourse(int userId);
        string UserInAcademy(int userId);

        bool IsTeacherInCourse(int TeacherId, int i);
        string TeacherInCourse(int TeacherId, int i);

        bool IsAcademyTypeInCourse(int AcademyId, int i);
        bool IsValidTimesAcademyInCourse(int AcademyId, int i);


        List<ShowClassEventsViewModel> GetEventOfClass(int CourseId);
        List<ShowClassEventsViewModel> GetEventOfTeachers(int TeacherId);
        List<BBBServers> GetListOfMoviesClass(int CourseId);
        List<ShowUserEventsViewModel> GetEventOfUsers(int userId);

        ReportForAdminViewModel GetReportForAdmin();
        ReportForAdminViewModel GetReportForAdmin(DateTime startDate , DateTime EndDate);

        List<ReportForSurvayDetailViewModel> GetReportForSurvay(PagingForSurvayDetailViewModel paging, ReportForSurvayViewModel model, int pageId = 1, int take = 10);
        List<ReportForSurvayDetailTeachersViewModel> GetReportForTeacherSurvay(PagingForSurvayDetailViewModel paging, ReportForTeacherViewModel model, int AcademyId, int pageId = 1, int take = 10);
        List<ReportForSurvayCommentsTeachersViewModel> GetCommentForTeacherSurvay(PagingForSurvayDetailViewModel paging, ReportForTeacherViewModel model, int pageId = 1, int take = 10);
        int[] GetTeachersAcademyForSurvay(ReportForTeacherViewModel model);

        int[] GetTeachersAcademyForSurvayPrint(int TeacherId , string StartDate , string EndDate);

        List<ReportForSurvayDetailViewModel> GetReportForSurvay(int StateId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId);

        List<ReportForSurvayDetailTeachersViewModel> GetReportForTeacherSurvay(int TeacherID, int AcademyId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId);
        List<ReportForSurvayCommentsTeachersViewModel> GetCommentForTeacherSurvay(int TeacherID, string StartDate, string EndDate, int GenderId, int EmpId, int EduId);

        bool CheckForRedirectToPolling(int UserId);
    }
}
