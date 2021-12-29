using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models
{
    public class JoinModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        public string Id { get; set; }
        public int ServerId { get; set; }
        public string Mobile { get; set; }
        public string ClassName { get; set; }
    }
}
