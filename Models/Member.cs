using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Member")]
    public class Member
    {
        [Key, ForeignKey("User"), Column(Order = 0)]
        public string MemberUserName { get; set; }
        [StringLength(10)]
        public string Identity { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentID { get; set; }
        [StringLength(50)]
        public string Year { get; set; }
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Birth { get; set; }
        [Required, StringLength(50)]
        public string Phone { get; set; }

        // navigation properties

        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<ProgramsMembers> ProgramsMembers { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Charge> Charges { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}