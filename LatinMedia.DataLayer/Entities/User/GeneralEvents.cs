using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class GeneralEvents
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserIP { get; set; }

        public int EventId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Extra { get; set; }

        #region Relation
        public EventType EventType { get; set; }
        public User User { get; set; }
        #endregion
    }
}
