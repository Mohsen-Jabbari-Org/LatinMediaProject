using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Permissions;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace LatinMedia.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private LatinMediaContext _context;

        public PermissionService(LatinMediaContext context)
        {
            _context = context;
        }
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });

            }

            _context.SaveChanges();
        }


        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //-------Delete All Roles User ---------//
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            //-----Add New Roles To User ------------//

            AddRolesToUser(rolesId, userId);

        }

        public List<string> GetUserRoles(int userId)
        {
            return _context.UserRoles.Include(r => r.Role).Where(r => r.UserId == userId)
                                     .Select(r => r.Role.RoleTitle).ToList();
        }

        public void RemoveRolesUser(int userId)
        {
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));
            _context.SaveChanges();
        }


        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermissionsToRole(int roleId, List<int> permissions)
        {
            foreach (var item in permissions)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = item,
                });
            }

            _context.SaveChanges();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _context.RolePermissions.Where(p => p.RoleId == roleId)
                                           .Select(p => p.PermissionId).ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _context.RolePermissions.Where(p => p.RoleId == roleId).ToList()
                .ForEach(p => _context.RolePermissions.Remove(p));

            AddPermissionsToRole(roleId, permissions);

        }

        public bool CheckPermission(int permissionId, string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            List<int> UserRoles = _context.UserRoles
                .Where(u => u.UserId == userId).Select(u => u.RoleId).ToList();
            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId).Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public bool CheckUserIsRole(string mobile, int Id , int Zero)
        {
            if(_context.Users.Any(u => u.Mobile == mobile && u.UserId == Id && u.AcademyId == Zero))
            {
                int userId = _context.Users.Single(u => u.Mobile == mobile && u.UserId == Id && u.AcademyId == Zero).UserId;
                bool userRoles = _context.UserRoles.Any(u => u.UserId == userId);

                if (userRoles)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
            
        }

        public bool CheckUserIsRole(string mobile)
        {
            if(_context.Users.Any(u => u.Mobile == mobile))
            {
                int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
                bool userRoles = _context.UserRoles.Any(u => u.UserId == userId);
                if (userRoles)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public bool CheckUserIsRoleTeacher(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            bool userRoles = _context.UserRoles.Where(u => u.RoleId == 4).Any(u => u.UserId == userId);
            if (userRoles)
                return true;
            else
                return false;
        }

        public bool CheckUserIsRoleInspector(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            bool userRoles = _context.UserRoles.Where(u => u.RoleId == 6).Any(u => u.UserId == userId);
            if (userRoles)
                return true;
            else
                return false;
        }

        public bool CheckUserIsRoleSupport(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            bool userRoles = _context.UserRoles.Where(u => u.RoleId == 2).Any(u => u.UserId == userId);
            if (userRoles)
                return true;
            else
                return false;
        }
        public bool CheckUserIsRoleAcademy(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            bool userRoles = _context.UserRoles.Where(u => u.RoleId == 3).Any(u => u.UserId == userId);
            if (userRoles)
                return true;
            else
                return false;
        }

        public bool CheckUserIsRoleAdmin(string mobile)
        {
            int userId = _context.Users.Single(u => u.Mobile == mobile).UserId;
            bool userRoles = _context.UserRoles.Where(u => u.RoleId == 1).Any(u => u.UserId == userId);
            if (userRoles)
                return true;
            else
                return false;
        }


        public List<CourseGroup> GetCourseGroup()
        {
            return _context.CourseGroups.ToList();
        }

        public void AddCourseGroupToNewTeacher(List<int> courseGroupIds, int TeacherId)
        {
            foreach (int courseGroupId in courseGroupIds)
            {
                _context.TeacherGroups.Add(new TeacherGroup()
                {
                    GroupId = courseGroupId,
                    TeacherId = TeacherId
                });

            }

            _context.SaveChanges();
        }

        public void EditCourseGroupToNewTeacher(List<int> courseGroupIds, int TeacherId)
        {
            _context.TeacherGroups.Where(t => t.TeacherId == TeacherId).ToList().ForEach(t => _context.TeacherGroups.Remove(t));
            AddCourseGroupToNewTeacher(courseGroupIds, TeacherId);
        }
    }
}
