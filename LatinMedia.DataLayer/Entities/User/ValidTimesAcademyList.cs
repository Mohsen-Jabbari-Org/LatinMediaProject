using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class ValidTimesAcademyList
    {
        [Key]
        public int VTA_Id { get; set; }

        public int AcademyId { get; set; }

        public int ValidTimesCourseId { get; set; }

        #region Relations
        public Academy Academy { get; set; }
        public ValidTimesCourse ValidTimesCourse { get; set; }
        #endregion
    }
}
