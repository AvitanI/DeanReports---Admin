using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Programs")]
    public class Programs
    {
        [Key]
        public int ID { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

        // navigation properties

        public virtual ICollection<ProgramsMembers> ProgramsMembers { get; set; }
    }
}