using LatinMedia.DataLayer.Entities.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Teacher
{
    public class TeacherGroup
    {
        [Key]
        public int TG_Id { get; set; }
        public int GroupId { get; set; }
        public int TeacherId { get; set; }

        #region Relations
        public NewTeacher NewTeacher { get; set; }
        public CourseGroup CourseGroup { get; set; }
        #endregion
    }
}
