using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Teacher
{
    public class TeacherAcademy
    {
        [Key]
        public int TA_Id { get; set; }
        public int TeacherId { get; set; }
        public int AcademyId { get; set; }

        #region Relations

        public NewTeacher NewTeacher { get; set; }
        public Academy Academy { get; set; }

        #endregion
    }
}
