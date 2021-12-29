using LatinMedia.DataLayer.Entities.Teacher;
using LatinMedia.DataLayer.Entities.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int ServerId { get; set; }
        public string ClassId { get; set; }
        public string TeacherIP { get; set; }
        public int EventId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Extra { get; set; }

        #region Relation
        public EventType EventType { get; set; }
        public NewTeacher NewTeacher { get; set; }
        public Course.Course Course { get; set; }
        #endregion
    }
}
