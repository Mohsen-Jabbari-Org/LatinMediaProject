using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class EventType
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; }

        #region Relations
        public List<Event> Events { get; set; }
        public List<UserEvent> UserEvents { get; set; }
        public List<GeneralEvents> GeneralEvents { get; set; }
        #endregion
    }
}
