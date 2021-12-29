using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Genertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Company;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LatinMedia.Core.Services
{
   public class CourseService : ICourseService
   {
        private LatinMediaContext _context;
        private IHostingEnvironment _environment;

        public CourseService(LatinMediaContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public List<CourseGroup> GetAllGroups()
        {
            return _context.CourseGroups.ToList();
        }

        public List<State> GetStates()
        {
            return _context.States.ToList();
        }

        public List<SelectListItem> GetGroupsForManageCourse() // دریافت لیست گروه آموزش (آئین نامه یا فنی)
        {
            return _context.CourseGroups.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupsForManageCourse(int groupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSecondSubGroupsForManageCourse(int subGroupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == subGroupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetTeachersForManageCourse()
        {
            return _context.Teachers.Select(t => new SelectListItem()
            {
                Value = t.TeacherId.ToString(),
                Text = t.TeacherNameFa,
            }).ToList();
        }

        public List<SelectListItem> GetTeachersFromUsersToManageCourse(int AcademyId)
        {
            return _context.TeacherAcademies.Where(ta => ta.AcademyId == AcademyId).Select(ta => new SelectListItem()
            {
                Value = ta.TeacherId.ToString(),
                Text = ta.NewTeacher.Gender.GenderName + " " + ta.NewTeacher.FirstName + " " + ta.NewTeacher.LastName,
            }).ToList();
        }

        public List<SelectListItem> GetTeachersFromCityToAddAcademy(int CityId , int AcademyId)
        {
            var TeacherIds = _context.TeacherAcademies.Where(ta => ta.AcademyId == AcademyId).Select(ta => ta.TeacherId).ToArray();
            int StateId = _context.Cities.Where(c => c.CityId == CityId).Select(c => c.StateId).First();
            var Cities = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            List<SelectListItem> test = 
            _context.NewTeachers.Where(n => Cities.Contains(n.CityCode) && !TeacherIds.Contains(n.TeacherId)).Select(n => new SelectListItem()
            {
                Value = n.TeacherId.ToString(),
                Text = n.Gender.GenderName + " " + n.FirstName + " " + n.LastName,
            }).ToList();
            return test;
        }

        public List<SelectListItem> GetTeachersForSupportToAddAcademy(int StateId, int AcademyId)
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            var TeacherIds = _context.TeacherAcademies.Where(ta => ta.AcademyId == AcademyId).Select(ta => ta.TeacherId).ToArray();
            List<SelectListItem> test =
            _context.NewTeachers.Where(n => cityIds.Contains(n.CityCode) && !TeacherIds.Contains(n.TeacherId)).Select(n => new SelectListItem()
            {
                Value = n.TeacherId.ToString(),
                Text = n.Gender.GenderName + " " + n.FirstName + " " + n.LastName,
            }).ToList();
            return test;
        }

        public int[] GetTeachersFromSatate(int StateId)
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            int[] test =
            _context.NewTeachers.Where(n => cityIds.Contains(n.CityCode)).Select(n => n.TeacherId).ToArray();
            return test;
        }

        public List<SelectListItem> GetTeachersFromUsersToManageCourse()
        {
            var userIds = _context.UserRoles.Where(ur => ur.RoleId == 4).Select(ur => ur.UserId).ToArray();
            return _context.Users.Where(t => userIds.Contains(t.UserId)).Select(t => new SelectListItem()
            {
                Value = t.UserId.ToString(),
                Text = t.Gender.GenderName + " " + t.FirstName + " " + t.LastName,
            }).ToList();
        }
        public List<SelectListItem> GetUsersFromUsersToManageCourse()
        {
            return _context.Users.Where(t => t.UserType == 0  && t.IsActive == true && t.IsDelete == false).Select(t => new SelectListItem()
            {
                Value = t.UserId.ToString(),
                Text = t.Gender.GenderName + " " + t.FirstName + " " + t.LastName,
            }).ToList();
        }

        public List<SelectListItem> GetUsersFromUsersToManageCourse(int id , int companyId)
        {
            var userIds = _context.userCourses.Where(uc => uc.CourseId == id).Select(uc => uc.UserId).ToArray();
            var users = _context.UserRoles.Select(ur => ur.UserId).ToArray();
            return _context.Users.Where(t => !userIds.Contains(t.UserId) && t.AcademyId == companyId && !users.Contains(t.UserId) && t.IsActive == true
            && t.IsDelete == false).Select(t => new SelectListItem()
            {
                Value = t.UserId.ToString(),
                Text = t.Gender.GenderName + " " + t.FirstName + " " + t.LastName,
            }).ToList();
        }

        public List<SelectListItem> GetLevelsForManageCourse()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Value = l.LevelId.ToString(),
                Text = l.LevelTitle
            }).ToList();
        }

        public List<SelectListItem> GetCompaniesForManageCourse()
        {
            return _context.Companies.Select(c => new SelectListItem()
            {
                Value = c.CompanyId.ToString(),
                Text = c.CompanyTitle
            }).ToList();
        }

        public List<SelectListItem> GetValidTimesForManageCourse(int academyId)
        {
            return _context.ValidTimesAcademyLists.Where(v => v.AcademyId == academyId).Select(v => new SelectListItem()
            {
                Value = v.ValidTimesCourse.ValidTimesCourseId.ToString(),
                Text = v.ValidTimesCourse.ValidTimesCourseName
            }).OrderBy(v => v.Text).ToList();
        }

        public List<SelectListItem> GetAcademiesForManageCourse()
        {
            return _context.Academies.Select(c => new SelectListItem()
            {
                Value = c.AcademyId.ToString(),
                Text = c.AcademyFullName
            }).ToList();
        }

        public List<SelectListItem> GetAcademiesForSupportCourse(int stateId)
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == stateId).Select(c => c.CityId).ToArray();
            return _context.Academies.Select(c => new SelectListItem()
            {
                Value = c.AcademyId.ToString(),
                Text = c.AcademyFullName
            }).ToList();
        }
        public List<SelectListItem> GetCourseTypesForManageCourse()
        {
            return _context.CourseTypes.Select(t => new SelectListItem()
            {
                Value = t.TypeId.ToString(),
                Text = t.TypeTitle,
            }).ToList();
        }

        public string GetTimesForCourse(int VTA_Id)
        {
            string[] time = _context.ValidTimesAcademyLists.Where(v => v.ValidTimesCourseId == VTA_Id).Select(v => v.ValidTimesCourse.ValidTimesCourseName).ToArray();
            return time[0];
        }

        public string GetGroupNameForAdmin(int GroupId)
        {
            string[] groupName = _context.CourseGroups.Where(c => c.GroupId == GroupId).Select(c => c.GroupTitle).ToArray();
            return groupName[0];
        }

        public string GetTimesForArchivedCourse(int VTA_Id)
        {
            string[] time = _context.ValidTimesCourses.Where(v => v.ValidTimesCourseId == VTA_Id).Select(v => v.ValidTimesCourseName).ToArray();
            return time[0];
        }

        public string GetStartTimes(int VTA_Id)
        {
            string[] time = _context.ValidTimesAcademyLists.Where(v => v.ValidTimesCourseId == VTA_Id).Select(v => v.ValidTimesCourse.StartTime).ToArray();
            return time[0];
        }
        public string GetEndTimes(int VTA_Id)
        {
            string[] time = _context.ValidTimesAcademyLists.Where(v => v.ValidTimesCourseId == VTA_Id).Select(v => v.ValidTimesCourse.EndTime).ToArray();
            return time[0];
        }
        public List<ShowCourseForAdminViewModel> GetCoursesForAdmin()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                EndDate = c.EndDate,
                ValidTimesCourseId = c.VTA_Id,
                AccId = _context.Academies.Where(a => a.AcademyId == c.CompanyId).Select(a => a.AcademyFullName).First(),
                CourseFaTitle = c.CourseFaTitle,
                //CourseType = c.CourseType.TypeTitle,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                CourseId = c.CourseId,
                ClassId = c.DemoFileName,
                CourseLatinTitle = c.CourseLatinTitle,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle
            }).ToList();
        }

        public CourseListForAdminViewModel GetCoursesForAdmin(int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "")
        {
            IQueryable<Course> result = _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) < 0);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.CourseFaTitle.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByAcademy))
            {
                result = result.Where(u => u.CompanyId == Int32.Parse(filterByAcademy));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            CourseListForAdminViewModel list = new CourseListForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Courses = result.OrderBy(u => u.Academy.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.CourseCounts = result.Count();
            return list;
        }

        public CourseListForAdminViewModel GetCoursesForSupport(int StateId, int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            int[] AcademyIds = _context.Academies.Where(a => cityIds.Contains(a.CityId)).Select(a => a.AcademyId).ToArray();
            IQueryable<Course> result = _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) < 0 && AcademyIds.Contains(t.CompanyId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.CourseFaTitle.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByAcademy))
            {
                result = result.Where(u => u.CompanyId == Int32.Parse(filterByAcademy));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            CourseListForAdminViewModel list = new CourseListForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Courses = result.OrderBy(u => u.Academy.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.CourseCounts = result.Count();
            return list;
        }

        public CourseListForAdminViewModel GetFinishedCoursesForAdmin(int pageId = 1, int take = 10, string filterByName = "", string filterByAcademy = "")
        {
            IQueryable<Course> result = _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) > 0);  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.CourseFaTitle.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByAcademy))
            {
                result = result.Where(u => u.CompanyId == Int32.Parse(filterByAcademy));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            CourseListForAdminViewModel list = new CourseListForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Courses = result.OrderBy(u => u.Academy.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.CourseCounts = result.Count();
            return list;
        }

        public CourseListForAdminViewModel GetFinishedCoursesForSupport(int StateId, int pageId = 1, int take = 10, string filterByName = "", string filterByCity = "")
        {
            int[] cityIds = _context.Cities.Where(c => c.StateId == StateId).Select(c => c.CityId).ToArray();
            int[] AcademyIds = _context.Academies.Where(a => cityIds.Contains(a.CityId)).Select(a => a.AcademyId).ToArray();
            IQueryable<Course> result = _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) > 0 && AcademyIds.Contains(t.CompanyId));  //lazyLoad;

            if (!string.IsNullOrEmpty(filterByName))
            {
                result = result.Where(u => u.CourseFaTitle.Contains(filterByName));
            }

            if (!string.IsNullOrEmpty(filterByCity))
            {
                result = result.Where(u => u.Academy.CityId == Int32.Parse(filterByCity));
            }

            int takeData = take;
            int skip = (pageId - 1) * takeData;

            CourseListForAdminViewModel list = new CourseListForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takeData);
            list.Courses = result.OrderBy(u => u.Academy.AcademyFullName).Skip(skip).Take(takeData).ToList();
            list.CourseCounts = result.Count();
            return list;
        }

        public List<ShowCourseForAdminViewModel> GetCoursesForAcUsers(int AcademyId)
        {
            return _context.Courses.Where(c => c.CompanyId == AcademyId && DateTime.Compare(DateTime.Now,c.EndDate) <= 0).Select(c => new ShowCourseForAdminViewModel()
            {
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                EndDate = c.EndDate,
                ValidTimesCourseId = c.VTA_Id,
                AccId = _context.Academies.Where(a => a.AcademyId == c.CompanyId).Select(a => a.AcademyFullName).First(),
                CourseFaTitle = c.CourseFaTitle,
                //CourseType = c.CourseType.TypeTitle,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                CourseId = c.CourseId,
                ClassId = c.DemoFileName,
                CourseLatinTitle = c.CourseLatinTitle,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle
            }).ToList();
        }

        public List<ShowCourseForAdminViewModel> GetArchivedCoursesForAcUsers(int AcademyId)
        {
            return _context.Courses.Where(c => c.CompanyId == AcademyId && DateTime.Compare(DateTime.Now, c.EndDate) > 0).Select(c => new ShowCourseForAdminViewModel()
            {
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                EndDate = c.EndDate,
                ValidTimesCourseId = c.VTA_Id,
                AccId = _context.Academies.Where(a => a.AcademyId == c.CompanyId).Select(a => a.AcademyFullName).First(),
                CourseFaTitle = c.CourseFaTitle,
                //CourseType = c.CourseType.TypeTitle,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                CourseId = c.CourseId,
                ClassId = c.DemoFileName,
                CourseLatinTitle = c.CourseLatinTitle,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle
            }).ToList();
        }

        public List<ShowCourseForAdminViewModel> GetCoursesForAcUsers()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                EndDate = c.EndDate,
                ValidTimesCourseId = c.VTA_Id,
                AccId = _context.Academies.Where(a => a.AcademyId == c.CompanyId).Select(a => a.AcademyFullName).First(),
                CourseFaTitle = c.CourseFaTitle,
                //CourseType = c.CourseType.TypeTitle,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                CourseId = c.CourseId,
                ClassId = c.DemoFileName,
                CourseLatinTitle = c.CourseLatinTitle,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle
            }).ToList();
        }

        public int AddCourse(Course course, IFormFile imgCourseUp, IFormFile courseFile)
        {
            //course.CreateDate = DateTime.Now;
            course.CourseImageName = "online.jpg";
            course.CourseFileName = "No File";

            if (imgCourseUp != null && imgCourseUp.IsImage())
            {
                string imagePath = "";

                course.CourseImageName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(imgCourseUp.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourseUp.CopyTo(stream);
                }
                //-------Save Thumbnail Course Image----------//

                string thumbPath = Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                ImageConvertors imgResizer = new ImageConvertors();
                imgResizer.Image_resize(imagePath, thumbPath, 200);

            }

            if (courseFile != null)
            {
                string filePath = "";

                course.CourseFileName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(courseFile.FileName);
                filePath = Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    courseFile.CopyTo(stream);
                }
            }

            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.CourseId;
        }

        public void DeleteCourse(int CourseId)
        {
            Course course = GetCourseById(CourseId);
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseFile)
        {
            var currentDate = course.CreateDate;

            if (imgCourse != null && imgCourse.IsImage())
            {
                string imagePath = "";

                #region Remove Old Course Images

                if (course.CourseImageName != "no-photo.png")
                {
                    string deletePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                    //------Delete Thumb Course Image ------//
                    string deleteThumbPath = Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                    if (File.Exists(deleteThumbPath))
                    {
                        File.Delete(deleteThumbPath);
                    }
                }

                #endregion

                #region Add New Course Image
                course.CourseImageName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(imgCourse.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }



                string thumbPath = Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                ImageConvertors imgResizer = new ImageConvertors();
                imgResizer.Image_resize(imagePath, thumbPath, 150);


                #endregion

            }


            if (courseFile != null)
            {
                string filePath = "";

                string deleteFilePath = Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                if (File.Exists(deleteFilePath))
                {
                    File.Delete(deleteFilePath);
                }

                course.CourseFileName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(courseFile.FileName);
                filePath = Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    courseFile.CopyTo(stream);
                }
            }

            //course.CreateDate = currentDate;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public Course GetCourseById(int id)
        {
            return _context.Courses.Find(id);
        }

        public Course GetCourseByMeetingId(string MeetingId)
        {
            return _context.Courses.SingleOrDefault(c => c.DemoFileName == MeetingId);
        }

        public Tuple<List<ShowCourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", int type = 0, int minPrice = 0, 
            int maxPrice = 0, List<int> selectedGroups = null, int take = 0, int companyId = 0, int teacherId = 0)
        {
            IQueryable<Course> result = _context.Courses;

            if (take == 0)
                take = 8;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => (c.CourseFaTitle.Contains(filter) || c.CourseLatinTitle.Contains(filter)));
            }

            //if (type > 0)
            //{
            //    result = result.Where(c => c.TypeId == type);
            //}

            if (minPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > minPrice);
            }

            if (maxPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < maxPrice);
            }

            if (selectedGroups != null)
            {
                foreach (var groupId in selectedGroups)
                {
                    result = result.Where(c =>
                        c.GroupId == groupId || c.SubGroupId == groupId || c.SecondSubGroupId == groupId);

                }
            }
            if (companyId > 0)
            {
                result = result.Where(c => c.CompanyId == companyId);
            }

            if (teacherId > 0)
            {
                result = result.Where(c => c.TeacherId == teacherId);
            }

            int skip = (pageId - 1) * take;
            int pageCount = (int) Math.Ceiling(result.Include(c => c.Teacher).Include(c => c.Academy).Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                Company = c.Academy.AcademyName,
                Teacher = c.Teacher.TeacherNameFa,
                CourseImage = c.CourseImageName,
                CourseTitle = c.CourseFaTitle,
                TeacherImage = c.Teacher.TeacherImageName,

            }).Count() / (double) take);
            var query = result.Include(c => c.Teacher).Include(c => c.Academy).Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                CoursePrice = c.CoursePrice,
                CourseTime = c.CourseTime,
                Company = c.Academy.AcademyName,
                Teacher = c.Teacher.TeacherNameFa,
                CourseImage = c.CourseImageName,
                CourseTitle = c.CourseFaTitle,
                TeacherImage = c.Teacher.TeacherImageName,

            }).Skip(skip).Take(take).ToList();
            return Tuple.Create(query, pageCount);
        }

        public List<CourseType> GetAllCourseTypes()
        {
            return _context.CourseTypes.ToList();
        }

        public string GetAcademyById(int AcademyId)
        {
            return _context.Academies.Where(a => a.AcademyId == AcademyId).Select(a => a.AcademyFullName).Single();
        }

        public List<Teacher> GetAllTeachers()
        {
            return _context.Teachers.ToList();
        }

        public List<ShowAllTeachersViewModel> ShowAllTeachers()
        {
            return _context.Teachers.Select(t => new ShowAllTeachersViewModel()
            {
                TeacherId = t.TeacherId,
                TeacherNameFa = t.TeacherNameFa,
                TeacherNameEN = t.TeacherNameEN,
                Description = t.Description,
                TeacherImageName = t.TeacherImageName,
                CourseCount = t.Courses.Count(c => c.TeacherId == t.TeacherId)
            }).ToList();
        }

        public List<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        //public List<ShowAllCompaniesViewModel> ShowAllCompanies()
        //{
        //    return _context.Companies.Select(c => new ShowAllCompaniesViewModel()
        //    {
        //        CompanyId = c.CompanyId,
        //        CompanyTitle = c.CompanyTitle,
        //        CompanyImageName = c.CompanyImageName,
        //        CourseCount = c.Courses.Count(g => g.CompanyId == c.CompanyId)
        //    }).ToList();
        //}

        public Course GetCourseForShow(int id)
        {
            return _context.Courses.Include(c => c.Academy)
                //.Include(c => c.CourseLevel)
                .Include(c => c.Teacher)
                //.Include(c => c.CourseLevel)
                .Include(c => c.CourseGroup)
                .Include(c => c.SubGroup)
                .Include(c => c.SecondSubGroup)
                .Include(c => c.UserCourses)
                .FirstOrDefault(c => c.CourseId == id);
        }

        public void AddComment(CourseComment comment)
        {
            _context.CourseComments.Add(comment);
            _context.SaveChanges();
        }

        public Tuple<List<CourseComment>, int> GetCourseComments(int courseId, int pageId = 1)
        {
            int take = 6;
            int skip = (pageId - 1) * take;
            int pageCount = (int)Math.Ceiling(_context.CourseComments
                .Include(c => c.User)
                .Count(c => c.CourseId == courseId && !c.IsDelete) / (double)take);

            var result = _context.CourseComments.Include(c => c.User)
                .Where(c => c.CourseId == courseId && !c.IsDelete).OrderByDescending(c => c.CreateDate)
                .Skip(skip).Take(take).ToList();
            return Tuple.Create(result, pageCount);
        }

        public void AddUserInClass(int userId, int courseId)
        {
            if(!_context.UserCourses.Any(c => c.UserId == userId && c.CourseId == courseId))
            {
                _context.userCourses.Add(new UserCourse()
                {
                    CourseId = courseId,
                    UserId = userId,
                });
                _context.SaveChanges();
            }
        }

        public void AddTeacherInAcademy(int TeacherId, int AcademyId)
        {
            if (!_context.TeacherAcademies.Any(t => t.TeacherId == TeacherId && t.AcademyId == AcademyId))
            {
                _context.TeacherAcademies.Add(new TeacherAcademy()
                {
                    AcademyId = AcademyId,
                    TeacherId = TeacherId,
                });
                _context.SaveChanges();
            }
        }

        public void RemoveUserFromClass(int userCourseId, int courseId)
        {
            if (_context.UserCourses.Any(c => c.UC_Id == userCourseId && c.CourseId == courseId))
            {
                _context.userCourses.Remove(new UserCourse()
                {
                    CourseId = courseId,
                    UC_Id = userCourseId,
                });
                _context.SaveChanges();
            }
        }

        public bool RemoveTeacherFromAcademy(int TA_Id, int AcademyId)
        {
            int TeacherId = _context.TeacherAcademies.Where(ta => ta.TA_Id == TA_Id).Select(ta => ta.TeacherId).First();
            if (_context.TeacherAcademies.Any(t => t.TA_Id == TA_Id && t.AcademyId == AcademyId))
            {
                if (_context.Courses.Any(c => DateTime.Compare(c.EndDate, DateTime.Now) > 0 && c.CompanyId == AcademyId && c.TeacherId == TeacherId))
                {
                    return false;
                }
                else
                {
                    _context.TeacherAcademies.Remove(new TeacherAcademy()
                    {
                        AcademyId = AcademyId,
                        TA_Id = TA_Id,
                    });
                    _context.SaveChanges();
                }
            }
            return true;
        }

        public List<UserCourse> GetUserCourse(int Cid)
        {
            return _context.userCourses.Where(uc => uc.CourseId == Cid).Include(uc => uc.User).IgnoreQueryFilters().ToList();
        }

        public List<TeacherAcademy> GetAcademiesTeachers(int Cid)
        {
            return _context.TeacherAcademies.Where(ta => ta.AcademyId == Cid).Include(ta => ta.NewTeacher).ToList();
        }

        public List<ShowUsersInClassViewModel> GetUsersInClass(int CourseId)
        {
            return _context.Users.IgnoreQueryFilters().Join(_context.userCourses,
                                           user => user.UserId,
                                           usercourse => usercourse.User.UserId,
                                           (user, usercourse) => new ShowUsersInClassViewModel()
                                           {
                                               FullName = user.Gender.GenderName + " " + user.FirstName + " " + user.LastName,
                                               Mobile = user.Mobile,
                                               Value = usercourse.CourseId,
                                               CourseName = usercourse.Course.CourseFaTitle,
                                               UserAvatar = user.UserAvatar,
                                               CreateDate = user.CreateDate
                                           }).Where(c => c.Value == CourseId).ToList();
        }

        public BBBServers GetServerParameters(int academyId)
        {
            var serverId = _context.Academies.Where(a => a.AcademyId == academyId).Select(a => a.BBBServers.Id).ToArray();
            return _context.BBBServers.Where(b => serverId.Contains(b.Id)).Single();
        }

        public List<BBBServers> GetAllServers()
        {
            return _context.BBBServers.Where(b => b.IsActive).ToList();
        }

        public BBBServers GetServerTotalParameters(int serverId)
        {
            return _context.BBBServers.Where(b => b.Id == serverId).Single();
        }

        public bool IsUserInCourse(int userId)
        {
            return _context.UserCourses.Any(uc => uc.UserId == userId);
        }

        public string UserInAcademy(int userId)
        {
            int academyId = _context.UserCourses.Where(uc => uc.UserId == userId).Select(uc => uc.Course.CompanyId).First();
            return _context.Academies.Where(a => a.AcademyId == academyId).Select(a => a.AcademyFullName).First();
        }

        public bool IsTeacherInCourse(int TeacherId , int i)
        {
            return _context.Courses.Any(c => DateTime.Compare(c.EndDate, DateTime.Now) > 0 && c.TeacherId == TeacherId && c.GroupId == i);
        }

        public bool IsAcademyTypeInCourse(int AcademyId, int i)
        {
            return _context.Courses.Any(c => DateTime.Compare(c.EndDate, DateTime.Now) > 0 && c.CompanyId == AcademyId && c.GroupId == i);
        }

        public bool IsValidTimesAcademyInCourse(int AcademyId, int i)
        {
            return _context.Courses.Any(c => DateTime.Compare(DateTime.Now, c.EndDate) < 0 && c.CompanyId == AcademyId && c.VTA_Id == i);
        }

        public string TeacherInCourse(int TeacherId, int i)
        {
            int academyId = _context.Courses
                .Where(c => c.TeacherId == TeacherId && c.GroupId == i && DateTime.Compare(DateTime.Now, c.EndDate) < 0)
                .Select(c => c.CompanyId).First();
            return _context.Academies.Where(a => a.AcademyId == academyId).Select(a => a.AcademyFullName).First();
        }

        public List<ShowClassEventsViewModel> GetEventOfClass(int CourseId)
        {
            return _context.Events.Where(e => e.CourseId == CourseId).Select(e => new ShowClassEventsViewModel()
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseName = e.Course.CourseFaTitle,
                EventId = e.EventId,
                EventName = e.EventType.EventName,
                TeacherId = e.TeacherId,
                TeacherName = e.NewTeacher.Gender.GenderName + " " + e.NewTeacher.FirstName + " " + e.NewTeacher.LastName,
                CreateDate = e.CreateDate,
                Extra = e.Extra,
                TeacherIP = e.TeacherIP
            }).ToList();
        }

        public List<ShowClassEventsViewModel> GetEventOfTeachers(int TeacherId)
        {
            return _context.Events.Where(e => e.TeacherId == TeacherId).Select(e => new ShowClassEventsViewModel()
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseName = e.Course.CourseFaTitle,
                EventId = e.EventId,
                EventName = e.EventType.EventName,
                TeacherId = e.TeacherId,
                TeacherName = e.NewTeacher.Gender.GenderName + " " + e.NewTeacher.FirstName + " " + e.NewTeacher.LastName,
                CreateDate = e.CreateDate,
                Extra = e.Extra,
                TeacherIP = e.TeacherIP
            }).ToList();
        }

        public List<BBBServers> GetListOfMoviesClass(int CourseId)
        {
            var Servers = _context.Events.Where(e => e.CourseId == CourseId && e.EventId == 1).Select(e => e.ServerId).ToArray();
            return _context.BBBServers.Where(b => Servers.Contains(b.Id)).ToList();
            //return _context.Events.Where(e => e.CourseId == CourseId && e.EventId == 1).Select(e => new ClassMoviesViewModel()
            //{
            //    Id = e.Id,
            //    CourseId = e.CourseId,
            //    ClassId = e.Course.DemoFileName,
            //    ClassName = _context.Courses.Where(c => c.CourseId == CourseId).Select(c => c.CourseFaTitle).Single(),
            //    ServerId = e.ServerId,
            //    ServerName = _context.BBBServers.Where(b => b.Id == e.ServerId).Select(b => b.ServerName).Single(),
            //    CreateDate = e.CreateDate
            //}).ToList();
        }

        public List<ShowUserEventsViewModel> GetEventOfUsers(int userId)
        {
            return _context.UserEvents.Where(e => e.UserId == userId).Select(e => new ShowUserEventsViewModel()
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseName = e.Course.CourseFaTitle,
                EventId = e.EventId,
                EventName = e.EventType.EventName,
                UserId = e.UserId,
                UserFullName = e.User.Gender.GenderName + " " + e.User.FirstName + " " + e.User.LastName,
                CreateDate = e.CreateDate,
                Extra = e.Extra,
                UserIP = e.UserIP
            }).ToList();
        }

        public ReportForAdminViewModel GetReportForAdmin()
        {
            ReportForAdminViewModel model = new ReportForAdminViewModel();
            model.UserCounts = _context.Users.ToList().Count;
            model.TeacherCounts = _context.NewTeachers.ToList().Count;
            var FinishedClass = 
                _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) >= 0)
                .GroupBy(t => t.CourseId).Select(g => new { cnt = g.Sum(t => EF.Functions.DateDiffDay(t.CreateDate, t.EndDate) + 1) }).ToList();
            model.FinishedClass = FinishedClass.Sum(c => c.cnt);
            var CurruntClass =
                _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(DateTime.Now, t.EndDate) <= 0)
                .GroupBy(t => t.CourseId).Select(g => new { cnt = g.Sum(t => EF.Functions.DateDiffDay(t.CreateDate, t.EndDate) + 1) }).ToList();
            model.CurruntClass = CurruntClass.Sum(c => c.cnt);
            return model;
        }

        public ReportForAdminViewModel GetReportForAdmin(DateTime startDate, DateTime EndDate)
        {
            ReportForAdminViewModel model = new ReportForAdminViewModel();
            model.UserCounts = _context.Users.Where(u => u.CreateDate >= startDate && u.CreateDate <= EndDate).ToList().Count;
            model.TeacherCounts = _context.NewTeachers.Where(u => u.CreateDate >= startDate && u.CreateDate <= EndDate).ToList().Count;
            var FinishedClass =
                _context.Courses.Where(t => t.IsDelete == false &&  DateTime.Compare(startDate, t.EndDate) <= 0 && DateTime.Compare(EndDate, t.EndDate) >= 0)
                .GroupBy(t => t.CourseId).Select(g => new { cnt = g.Sum(t => EF.Functions.DateDiffDay(t.CreateDate, t.EndDate) + 1) }).ToList();
            model.FinishedClass = FinishedClass.Sum(c => c.cnt);
            var CurruntClass =
                _context.Courses.Where(t => t.IsDelete == false && DateTime.Compare(t.EndDate, EndDate) >= 0 && DateTime.Compare(t.CreateDate, startDate) <= 0)
                .GroupBy(t => t.CourseId).Select(g => new { cnt = g.Sum(t => EF.Functions.DateDiffDay(t.CreateDate, t.EndDate) + 1) }).ToList();
            model.CurruntClass = CurruntClass.Sum(c => c.cnt);
            return model;
        }

        //public List<ReportForSurvayDetailViewModel> GetReportForSurvay(int StateId)
        //{
        //    int[] TeachersForState = GetTeachersFromSatate(StateId);
        //    List<ReportForSurvayDetailViewModel> List = _context.SurvayDetails.Where(sd => TeachersForState.Contains(sd.TeacherId)).Select(sd => new ReportForSurvayDetailViewModel
        //    {
        //        TeacherId = sd.TeacherId,
        //        TeacherFullName = _context.NewTeachers.Where(t => t.TeacherId == sd.TeacherId).Select(t => new
        //        {
        //            FullName = t.Gender.GenderName + " " + t.FirstName + " " + t.LastName
        //        }).First().ToString(),
        //        Poll1 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll1).Count(),
        //        Poll2 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll2).Count(),
        //        Poll3 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll3).Count(),
        //        Poll4 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll4).Count(),
        //        Poll5 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll5).Count(),
        //        Poll6 = _context.SurvayDetails.Where(sd1 => sd1.TeacherId == sd.TeacherId).Select(sd1 => sd1.Poll6).Count()
        //    }).Distinct().ToList();
        //    return List;
        //}

        public List<ViewModels.ReportForSurvayDetailViewModel> GetReportForSurvay(PagingForSurvayDetailViewModel paging, ReportForSurvayViewModel model, int pageId = 1, int take = 10)
        {
            List<ViewModels.ReportForSurvayDetailViewModel> List = _context.PollReport(model.StateId, model.StartDate, model.EndDate , model.GenderId , model.EmpId , model.EduId).Select(pr => new ViewModels.ReportForSurvayDetailViewModel
            {
                TeacherId = pr.TeacherId,
                TeacherFullName = pr.TeacherFullName,
                Poll1_1 = Math.Round(((double)pr.Poll1_1 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_2 = Math.Round(((double)pr.Poll1_2 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_3 = Math.Round(((double)pr.Poll1_3 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_4 = Math.Round(((double)pr.Poll1_4 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_1 = Math.Round(((double)pr.Poll2_1 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_2 = Math.Round(((double)pr.Poll2_2 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_3 = Math.Round(((double)pr.Poll2_3 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_4 = Math.Round(((double)pr.Poll2_4 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_1 = Math.Round(((double)pr.Poll3_1 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_2 = Math.Round(((double)pr.Poll3_2 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_3 = Math.Round(((double)pr.Poll3_3 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_4 = Math.Round(((double)pr.Poll3_4 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_1 = Math.Round(((double)pr.Poll4_1 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_2 = Math.Round(((double)pr.Poll4_2 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_3 = Math.Round(((double)pr.Poll4_3 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_4 = Math.Round(((double)pr.Poll4_4 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_1 = Math.Round(((double)pr.Poll5_1 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_2 = Math.Round(((double)pr.Poll5_2 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_3 = Math.Round(((double)pr.Poll5_3 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_4 = Math.Round(((double)pr.Poll5_4 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_1 = Math.Round(((double)pr.Poll6_1 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_2 = Math.Round(((double)pr.Poll6_2 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_3 = Math.Round(((double)pr.Poll6_3 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_4 = Math.Round(((double)pr.Poll6_4 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                UserCount = pr.UserCount
            }).ToList();
            int takeData = take;
            int skip = (pageId - 1) * takeData;
            paging.PageCount = (int)Math.Ceiling(List.Count() / (double)takeData);
            paging.CurrentPage = pageId;
            paging.LastPage = paging.PageCount;
            paging.PrevPage = Math.Max(pageId - 1, paging.CurrentPage);
            paging.NextPage = Math.Max(pageId + 1, paging.LastPage);
            List<ViewModels.ReportForSurvayDetailViewModel> TolatList = List.Skip(skip).Take(takeData).ToList();
            paging.UserCounts = List.Count();
            return TolatList;
        }

        public List<ViewModels.ReportForSurvayDetailTeachersViewModel> GetReportForTeacherSurvay(PagingForSurvayDetailViewModel paging, ReportForTeacherViewModel model, int AcademyId , int pageId = 1, int take = 10)
        {
            List<ViewModels.ReportForSurvayDetailTeachersViewModel> List = _context.PollTeacherReport(model.TeacherId, AcademyId , model.StartDate, model.EndDate, model.GenderId, model.EmpId, model.EduId).Select(pr => new ViewModels.ReportForSurvayDetailTeachersViewModel
            {
                AcademyId = pr.AcademyId,
                AcademyFullName = pr.AcademyFullName,
                Poll1_1 = Math.Round(((double)pr.Poll1_1 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_2 = Math.Round(((double)pr.Poll1_2 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_3 = Math.Round(((double)pr.Poll1_3 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_4 = Math.Round(((double)pr.Poll1_4 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_1 = Math.Round(((double)pr.Poll2_1 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_2 = Math.Round(((double)pr.Poll2_2 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_3 = Math.Round(((double)pr.Poll2_3 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_4 = Math.Round(((double)pr.Poll2_4 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_1 = Math.Round(((double)pr.Poll3_1 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_2 = Math.Round(((double)pr.Poll3_2 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_3 = Math.Round(((double)pr.Poll3_3 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_4 = Math.Round(((double)pr.Poll3_4 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_1 = Math.Round(((double)pr.Poll4_1 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_2 = Math.Round(((double)pr.Poll4_2 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_3 = Math.Round(((double)pr.Poll4_3 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_4 = Math.Round(((double)pr.Poll4_4 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_1 = Math.Round(((double)pr.Poll5_1 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_2 = Math.Round(((double)pr.Poll5_2 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_3 = Math.Round(((double)pr.Poll5_3 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_4 = Math.Round(((double)pr.Poll5_4 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_1 = Math.Round(((double)pr.Poll6_1 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_2 = Math.Round(((double)pr.Poll6_2 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_3 = Math.Round(((double)pr.Poll6_3 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_4 = Math.Round(((double)pr.Poll6_4 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                UserCount = pr.UserCount
            }).ToList();
            int takeData = take;
            int skip = (pageId - 1) * takeData;
            paging.PageCount = (int)Math.Ceiling(List.Count() / (double)takeData);
            paging.CurrentPage = pageId;
            paging.LastPage = paging.PageCount;
            paging.PrevPage = Math.Max(pageId - 1, paging.CurrentPage);
            paging.NextPage = Math.Max(pageId + 1, paging.LastPage);
            List<ViewModels.ReportForSurvayDetailTeachersViewModel> TolatList = List.Skip(skip).Take(takeData).ToList();
            paging.UserCounts = List.Count();
            return TolatList;
        }

        public List<ViewModels.ReportForSurvayCommentsTeachersViewModel> GetCommentForTeacherSurvay(PagingForSurvayDetailViewModel paging, ReportForTeacherViewModel model, int pageId = 1, int take = 10)
        {
            List<ViewModels.ReportForSurvayCommentsTeachersViewModel> List = _context.GetCommentTeacher(model.TeacherId, model.StartDate, model.EndDate, model.GenderId, model.EmpId, model.EduId).Select(pr => new ViewModels.ReportForSurvayCommentsTeachersViewModel
            {
                UserId = pr.UserId,
                SurvayId = pr.SurvayId,
                FullName = pr.FullName,
                Age = pr.Age,
                Mobile = pr.Mobile,
                AcademyFullName = pr.AcademyFullName,
                Comment = pr.Comment
            }).ToList();
            int takeData = take;
            int skip = (pageId - 1) * takeData;
            paging.PageCount = (int)Math.Ceiling(List.Count() / (double)takeData);
            paging.CurrentPage = pageId;
            paging.LastPage = paging.PageCount;
            paging.PrevPage = Math.Max(pageId - 1, paging.CurrentPage);
            paging.NextPage = Math.Max(pageId + 1, paging.LastPage);
            List<ViewModels.ReportForSurvayCommentsTeachersViewModel> TolatList = List.Skip(skip).Take(takeData).ToList();
            paging.UserCounts = List.Count();
            return TolatList;
        }

        public List<ReportForSurvayCommentsTeachersViewModel> GetCommentForTeacherSurvay(int TeacherID, string StartDate, string EndDate, int GenderId, int EmpId, int EduId)
        {
            List<ViewModels.ReportForSurvayCommentsTeachersViewModel> List = _context.GetCommentTeacher(TeacherID, StartDate, EndDate, GenderId, EmpId, EduId).Select(pr => new ViewModels.ReportForSurvayCommentsTeachersViewModel
            {
                UserId = pr.UserId,
                SurvayId = pr.SurvayId,
                FullName = pr.FullName,
                Age = pr.Age,
                Mobile = pr.Mobile,
                AcademyFullName = pr.AcademyFullName,
                Comment = pr.Comment
            }).ToList();
            return List;
        }

        public int[] GetTeachersAcademyForSurvay(ReportForTeacherViewModel model)
        {
            return _context.GetTeachersAcademy(model.TeacherId, model.StartDate, model.EndDate).Select(t => t.AcademyId).ToArray();
        }

        public int[] GetTeachersAcademyForSurvayPrint(int TeacherId, string StartDate, string EndDate)
        {
            return _context.GetTeachersAcademy(TeacherId, StartDate, EndDate).Select(t => t.AcademyId).ToArray();
        }

        public List<ViewModels.ReportForSurvayDetailViewModel> GetReportForSurvay(int StateId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId)
        {
            List<ViewModels.ReportForSurvayDetailViewModel> List = _context.PollReport(StateId, StartDate, EndDate, GenderId, EmpId, EduId).Select(pr => new ViewModels.ReportForSurvayDetailViewModel
            {
                TeacherId = pr.TeacherId,
                TeacherFullName = pr.TeacherFullName,
                Poll1_1 = Math.Round(((double)pr.Poll1_1 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_2 = Math.Round(((double)pr.Poll1_2 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_3 = Math.Round(((double)pr.Poll1_3 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_4 = Math.Round(((double)pr.Poll1_4 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_1 = Math.Round(((double)pr.Poll2_1 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_2 = Math.Round(((double)pr.Poll2_2 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_3 = Math.Round(((double)pr.Poll2_3 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_4 = Math.Round(((double)pr.Poll2_4 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_1 = Math.Round(((double)pr.Poll3_1 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_2 = Math.Round(((double)pr.Poll3_2 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_3 = Math.Round(((double)pr.Poll3_3 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_4 = Math.Round(((double)pr.Poll3_4 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_1 = Math.Round(((double)pr.Poll4_1 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_2 = Math.Round(((double)pr.Poll4_2 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_3 = Math.Round(((double)pr.Poll4_3 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_4 = Math.Round(((double)pr.Poll4_4 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_1 = Math.Round(((double)pr.Poll5_1 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_2 = Math.Round(((double)pr.Poll5_2 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_3 = Math.Round(((double)pr.Poll5_3 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_4 = Math.Round(((double)pr.Poll5_4 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_1 = Math.Round(((double)pr.Poll6_1 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_2 = Math.Round(((double)pr.Poll6_2 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_3 = Math.Round(((double)pr.Poll6_3 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_4 = Math.Round(((double)pr.Poll6_4 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
               UserCount = pr.UserCount
            }).ToList();
            return List;
        }

        public List<ViewModels.ReportForSurvayDetailTeachersViewModel> GetReportForTeacherSurvay(int TeacherID, int AcademyId, string StartDate, string EndDate, int GenderId, int EmpId, int EduId)
        {
            List<ViewModels.ReportForSurvayDetailTeachersViewModel> List = _context.PollTeacherReport(TeacherID, AcademyId, StartDate, EndDate, GenderId, EmpId, EduId).Select(pr => new ViewModels.ReportForSurvayDetailTeachersViewModel
            {
                AcademyId = pr.AcademyId,
                AcademyFullName = pr.AcademyFullName,
                Poll1_1 = Math.Round(((double)pr.Poll1_1 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_2 = Math.Round(((double)pr.Poll1_2 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_3 = Math.Round(((double)pr.Poll1_3 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll1_4 = Math.Round(((double)pr.Poll1_4 / ((double)pr.Poll1_1 + (double)pr.Poll1_2 + (double)pr.Poll1_3 + (double)pr.Poll1_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_1 = Math.Round(((double)pr.Poll2_1 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_2 = Math.Round(((double)pr.Poll2_2 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_3 = Math.Round(((double)pr.Poll2_3 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll2_4 = Math.Round(((double)pr.Poll2_4 / ((double)pr.Poll2_1 + (double)pr.Poll2_2 + (double)pr.Poll2_3 + (double)pr.Poll2_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_1 = Math.Round(((double)pr.Poll3_1 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_2 = Math.Round(((double)pr.Poll3_2 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_3 = Math.Round(((double)pr.Poll3_3 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll3_4 = Math.Round(((double)pr.Poll3_4 / ((double)pr.Poll3_1 + (double)pr.Poll3_2 + (double)pr.Poll3_3 + (double)pr.Poll3_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_1 = Math.Round(((double)pr.Poll4_1 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_2 = Math.Round(((double)pr.Poll4_2 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_3 = Math.Round(((double)pr.Poll4_3 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll4_4 = Math.Round(((double)pr.Poll4_4 / ((double)pr.Poll4_1 + (double)pr.Poll4_2 + (double)pr.Poll4_3 + (double)pr.Poll4_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_1 = Math.Round(((double)pr.Poll5_1 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_2 = Math.Round(((double)pr.Poll5_2 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_3 = Math.Round(((double)pr.Poll5_3 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll5_4 = Math.Round(((double)pr.Poll5_4 / ((double)pr.Poll5_1 + (double)pr.Poll5_2 + (double)pr.Poll5_3 + (double)pr.Poll5_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_1 = Math.Round(((double)pr.Poll6_1 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_2 = Math.Round(((double)pr.Poll6_2 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_3 = Math.Round(((double)pr.Poll6_3 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                Poll6_4 = Math.Round(((double)pr.Poll6_4 / ((double)pr.Poll6_1 + (double)pr.Poll6_2 + (double)pr.Poll6_3 + (double)pr.Poll6_4)), 3, MidpointRounding.ToEven) * 100,
                UserCount = pr.UserCount
            }).ToList();
            return List;
        }


        public bool CheckForRedirectToPolling(int UserId)
        {
            bool IsRolled = _context.UserRoles.Any(UR => UR.UserId == UserId);
            bool IsPolled = _context.Users.Any(U => U.UserId == UserId && U.IsPolled);
            if(!IsRolled && !IsPolled)
            {
                List<SortedClassViewModel> model = new List<SortedClassViewModel>();
                //int DateCounter = EF.Functions.DateDiffDay(StartDate, EndDate) + 1;
                int[] UCourse = _context.UserCourses.Where(uc => uc.UserId == UserId).Select(uc => uc.CourseId).ToArray();
                List<Course> courses = _context.Courses.Where(c => UCourse.Contains(c.CourseId)).OrderBy(c => c.CreateDate).ToList();
                int x = 1;
                for (int i = 0; i < courses.Count; i++)
                {
                    if (EF.Functions.DateDiffDay(courses[i].CreateDate, courses[i].EndDate) == 0)
                    {
                        model.Add(new SortedClassViewModel()
                        {
                            Counter = x,
                            CourseId = courses[i].CourseId,
                            ClassDate = courses[i].CreateDate
                        });
                        x = x + 1;
                    }
                    else
                    {
                        for (int j = 0; j <= EF.Functions.DateDiffDay(courses[i].CreateDate, courses[i].EndDate); j++)
                        {
                            model.Add(new SortedClassViewModel()
                            {
                                Counter = x,
                                CourseId = courses[i].CourseId,
                                ClassDate = courses[i].CreateDate.AddDays(j)
                            });
                            x = x + 1;
                        }
                    }

                }
                int flag = 0;
                foreach (SortedClassViewModel sorted in model)
                {
                    if (DateTime.Compare(sorted.ClassDate, DateTime.Now) < 0)
                        flag++;
                }
                if (flag >= 4)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
