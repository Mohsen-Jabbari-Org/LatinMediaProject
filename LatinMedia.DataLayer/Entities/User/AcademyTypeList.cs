using System;
using LatinMedia.DataLayer.Entities.Course;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class AcademyTypeList
    {
        [Key]
        public int AT_Id { get; set; }
        public int AcademyId { get; set; }
        public int AcademyTypeId { get; set; }

        #region Relations

        public AcademyType AcademyType { get; set; }
        public Academy Academy { get; set; }
        public Course.Course Course { get; set; }
        #endregion
    }
}
