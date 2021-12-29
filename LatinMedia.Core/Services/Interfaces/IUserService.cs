using System;
using System.Collections.Generic;
using System.Text;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Survay;

namespace LatinMedia.Core.Services.Interfaces
{
    public interface IUserService
    {
        #region Account
        bool IsExsitAcademy(int CityId , string FullName);
        bool IsExsitMobile(string mobile);
        bool IsExsitTeacherMobile(string mobile);
        int AddUser(User user);
        void AddSupportTell(string Tell, int StateId);
        int AddTeacher(NewTeacher newTeacher);
        List<Gender> Genders();
        List<Gender> GetGenders();
        List<Academy> Academies();
        List<Employment> Employments();
        List<Employment> GetEmployments();
        List<Education> Educations();
        List<Education> GetEducations();
        List<TwoItems> GetTwoItems();
        List<FourItems> GetFourItems();
        Academy GetAcademy(int Id);
        Gender GetGender(int Id);
        User LoginUser(LoginViewModel login);
        NewTeacher LoginTeacher(LoginTeacherViewModel login);
        User GetUserById(int userId);
        User GetDeleteUserById(int userId);
        NewTeacher GetTeacherById(int TeacherId);
        NewTeacher GetTeacherByTaId(int TeacherId);
        User GetUserByMobile(string mobile);
        NewTeacher GetTeacherByMobile(string mobile);
        User GetUserByActiveCode(string Mobile, string activeCode);
        NewTeacher GetTeacherByActiveCode(string Mobile, string activeCode);
        int GetUserIdByMobile(string mobile);
        int GetTeacherIdByMobile(string mobile);
        void UpdateUser(User user);
        void UpdateTeacher(NewTeacher newTeacher);
        bool ActiveAccount(string Mobile, string activeCode);
        bool ActiveTeacherAccount(string activeCode);
        bool ForgotTeacherActivateCode(string Mobile);
        bool ForgotActivateCode(string Mobile);
        #endregion

        #region Wallet

        int BalanceWalletUser(string mobile);
        List<WalletInfoViewModel> GetWalletUser(string mobile);
        int ChargeWallet(string mobile, int amount, string description, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);

        #endregion

        #region UserPanel

        InformationSupporterViewModel GetSupporterInformation(string mobile);
        InformationUserViewModel GetUserInformation(string mobile);
        InformationUserViewModel GetUserInformation(int Id);
        InformationUserViewModel GetDeleteUserInformation(int Id);
        InformationTeacherViewModel GetDeleteTeacherInformation(int Id);
        InformationTeacherViewModel GetTeacherInformation(string mobile);
        InformationInspectorViewModel GetInspectorInformation(string mobile);
        InformationTeacherViewModel GetTeacherInformation(int id);
        EditProfileViewModel GetDataForEditProfileUser(string mobile);
        SurveyViewModel GetDataForSurvay(string mobile);
        EditTeacherProfileViewModel GetDataForEditProfileTeacher(string mobile);
        EditInspectorProfileViewModel GetDataForEditProfileInspector(string mobile);
        EditSupportProfileViewModel GetDataForEditSupportProfileUser(string mobile);
        EditProfileAdminViewModel GetDataForEditAdminProfileUser(string mobile);
        void EditProfile(string mobile, EditProfileViewModel profile);
        void EditTeacherProfile(string mobile, EditTeacherProfileViewModel profile);
        void EditInspectorProfile(string mobile, EditInspectorProfileViewModel profile);
        void EditAdminProfile(string mobile, EditProfileAdminViewModel profile);
        void EditSupportProfile(string mobile, EditSupportProfileViewModel profile);
        bool CompareOldPassword(string mobile, string oldPassword);
        bool CompareTeacherOldPassword(string mobile, string oldPassword);
        void ChangeUserPassword(string mobile, string newPassword);
        void ChangeTeacherPassword(string mobile, string newPassword);


        #endregion

        #region Admin Panel

        SideBarAdminPanelViewModel GetSideBarAdminPanelData(string mobile);
        SideBarAcademyPanelViewModel GetSideBarAcademyPanelData(string mobile);
        TestUsersForAdminViewModel GetUsers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        TestUsersForAdminViewModel GetUsers(int AcId ,int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        UsersForAdminViewModel GetUsersForSupport(int StateId ,int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        UsersForAdminViewModel GetDeleteUsersForSupport(int stateId , int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        UsersForAdminViewModel GetDeleteUsers(int AcId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        void RestoreUsers(int userId);
        void RestoreTeachers(int teacherId);
        int AddAcademyFromAdmin(CreateAcademyViewModel model);
        EditAcademyViewModel GetAcademyForEdit(int AcademyId);
        int EditAcademyFromAdmin(EditAcademyViewModel model, int AcademyId);
        void UpdateCourseServer(int AcademyId, int ServerId);
        int AddUserFromAdmin(CreateUserViewModel model);
        int AddUserFromSupport(CreateUserViewModel model);
        void AddTypesToAcademy(List<int> TypesIds, int AcademyId);
        void EditTypesforAcademy(List<int> TypesIds, int AcademyId);
        void AddValidTimesToAcademy(List<int> ValidTimesIds, int AcademyId);
        void EditValidTimesForAcademy(List<int> ValidTimesIds, int AcademyId);
        int AddTeacherFromAdmin(CreateTeacherViewModel model);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        EditTeacherViewModel GetTeacherForShowInEditMode(int teacherId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        void EditTeacherFromAdmin(EditTeacherViewModel editTeacher);
        void DeleteUser(int userId);
        void DeleteTeacher(int teacherId);

        List<TeachersForSurvay> GetTeachers(int[] CourseId);
        TeachersForAdminViewModel GetTeachers(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        TeachersForAdminViewModel GetDeleteTeacher(int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        TeachersForAdminViewModel GetProvinceTeachers(int stateId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        TeachersForAdminViewModel GetProvinceDeleteTeacher(int stateId, int pageId = 1, int take = 10, string filterByLastName = "", string filterByMobile = "");
        AcademiesForAdminViewModel GetAcademies(int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "");
        AcademiesForAdminViewModel GetProvinceAcademies(int stateId, int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "");
        StatesForAdminViewModel GetStatesForAdmin(int pageId = 1, int take = 10, string filterByCity = "");
        CitiesForAdminViewModel GetCitiesForAdmin(int StateId, int pageId = 1, int take = 10, string filterByState = "");
        List<AcademyType> GetAcademyTypes();
        List<ValidTimesCourse> GetAcademyValidTimes();
        List<string> GetAcademyTypes(int academyId);
        List<string> GetAcademyValidTimes(int academyId);
        #endregion

        List<SelectListItem> GetState();
        List<SupportTell> GetStatesTells(int StateId);
        string GetStateNameById(int stateId);
        List<SelectListItem> GetCity(int stateId);
        List<SelectListItem> GetAcademies(int cityId);
        List<SelectListItem> GetAcademyName(int AcademyId);
        string GetAcademyFullName(int AcademyId);

        int GetCityForAcademies(int academyId);
        int GetMaxUsersForAcademies(int academyId);
        int GetStateFoAcademy(int cityId);
        string GetTeacherPlace(int cityId);
        string GetAcademyPlace(int academyId);
        List<CourseGroup> GetTeacherGroups();
        List<BBBServers> GetServers();
        BBBServerEditViewModel GetServerById(int Id);
        void UpdateBBBServer(BBBServerEditViewModel model , int id);
        int AddServer(BBBServers bBBServers);
        int AddCity(CreateCityViewModel createCityView);
        CityEditViewModel GetCityForEdit(int CityId);
        int EditCity(CityEditViewModel cityEdit, int CityId);
        void AddEvent(int TeacherId, int EventId, DateTime CreateDate, string TeacherIP, string Extra, int CourseId, int ServerId, string ClassId);
        void AddUserEvent(int UserId, int EventId, DateTime CreateDate, string UserIP, string Extra, int CourseId);
        void AddGeneralEvent(int UserId, int EventId, DateTime CreateDate, string UserIP, string Extra);
    }




}
