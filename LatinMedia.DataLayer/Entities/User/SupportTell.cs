using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class SupportTell
    {
        [Key]
        public int TellId { get; set; }
        public string Tell { get; set; }
        public int StateId { get; set; }


        #region Relation
        public State State { get; set; }

        #endregion
    }
}
