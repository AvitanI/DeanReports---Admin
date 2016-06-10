using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("ProgramsMembers")]
    public class ProgramsMembers
    {
        [Key, Column(Order = 0)]
        public string MemberUserName { get; set; }
        [Key, Column(Order = 1)]
        public int ProgramsID { get; set; }

        // navigation properties

        public virtual Member Member { get; set; }
        public virtual Programs Program { get; set; }
    }
}