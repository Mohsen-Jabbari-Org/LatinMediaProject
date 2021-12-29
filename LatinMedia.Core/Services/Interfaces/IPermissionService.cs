using System;
using System.Collections.Generic;
using System.Text;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Permissions;
using LatinMedia.DataLayer.Entities.User;

namespace LatinMedia.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Role
        List<Role> GetRoles();
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);
        List<string> GetUserRoles(int userId);
        void RemoveRolesUser(int userId);

        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        #endregion

        #region TeacherAcademy
        List<CourseGroup> GetCourseGroup();
        void AddCourseGroupToNewTeacher(List<int> courseGroupIds, int TeacherId);
        void EditCourseGroupToNewTeacher(List<int> courseGroupIds, int TeacherId);
        #endregion

        #region Permission

        List<Permission> GetAllPermissions();
        void AddPermissionsToRole(int roleId, List<int> permissions);
        List<int> PermissionsRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string mobile);
        bool CheckUserIsRole(string mobile);
        bool CheckUserIsRole(string mobile , int Id , int Zero);
        bool CheckUserIsRoleTeacher(string mobile);
        bool CheckUserIsRoleInspector(string mobile);
        bool CheckUserIsRoleSupport(string mobile);
        bool CheckUserIsRoleAcademy(string mobile);
        bool CheckUserIsRoleAdmin(string mobile);
        #endregion
    }
}
