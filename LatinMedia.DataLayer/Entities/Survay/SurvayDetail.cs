using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Survay
{
    public class SurvayDetail
    {
        public int SurvayId { get; set; }
        public int TeacherId { get; set; }
        public int Poll1 { get; set; }
        public int Poll2 { get; set; }
        public int Poll3 { get; set; }
        public int Poll4 { get; set; }
        public int Poll5 { get; set; }
        public int Poll6 { get; set; }
        public string Comment { get; set; }
        public DateTime SurvayDate { get; set; }

        #region Relation
        public Survay Survay { get; set; }
        #endregion
    }
}
