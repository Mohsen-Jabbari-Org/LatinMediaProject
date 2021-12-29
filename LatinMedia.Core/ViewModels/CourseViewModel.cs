using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.ViewModels
{
    public class ConfigServersForCourses
    {
        public int ServerId { get; set; }
        public string ClassId { get; set; }
    }
    public class ShowCourseForAdminViewModel
    {
        public int CourseId { get; set; }
        public string AccId { get; set; }
        //public string CourseType { get; set; }
        public string CourseFaTitle { get; set; }
        public string CourseLatinTitle { get; set; }
        public int CourseTime { get; set; }
        public int CoursePrice { get; set; }
        public string CourseImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ValidTimesCourseId { get; set; }
        public string GroupId { get; set; }
        public string SubGroupId { get; set; }
        public string SecondSubGroupId { get; set; }
        public string ClassId { get; set; }
    }

    public class ShowCourseListItemViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int CourseTime { get; set; }
        public int CoursePrice { get; set; }
        public string CourseImage { get; set; }
        public string Teacher { get; set; }
        public string TeacherImage { get; set; }
        public string Company { get; set; }

    }

    public class ShowUsersInClassViewModel
    {
        public int Value { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string CourseName { get; set; }
        public string UserAvatar { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ShowClassEventsViewModel
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherIP { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extra { get; set; }
    }

    public class ClassMoviesViewModel
    {
        public string name { get; set; }
        public string meetingID { get; set; }
        public string recordID { get; set; }
        public int size { get; set; }
        public string Url { get; set; }
    }

    public class ShowUserEventsViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string UserIP { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extra { get; set; }
    }
}
