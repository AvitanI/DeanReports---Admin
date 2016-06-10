using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Manager")]
    public class Manager
    {
        [Key, ForeignKey("User")]
        public string UserName { get; set; }
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Birth { get; set; }

        // navigation properties

        public virtual User User { get; set; }
    }
}