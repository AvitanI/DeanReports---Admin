using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Request")]
    public class Request
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, ForeignKey("Member"), Column(Order = 1)]
        public string StudentUserName { get; set; }
        [Required, StringLength(50)]
        public string Type { get; set; }
        [Required, StringLength(50)]
        public string Cause { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string FormType { get; set; }

        // internal use

        [ForeignKey("Manager")]
        public string ManagerUserName { get; set; }
        public string TeacherUserName { get; set; }
        [Range(0, 100)]
        public int? ApprovalHours { get; set; }
        public int? BudgetNumber { get; set; }
        [StringLength(250)]
        public string Notes { get; set; }
        public bool? ManagerSignature { get; set; }
        public DateTime? SignatureDate { get; set; }

        // navigation properties

        public virtual Member Member { get; set; }
        public virtual Manager Manager { get; set; }
        public virtual ICollection<CourseRequest> CourseRequests { get; set; }

        // instance methods

        public override string ToString()
        {
            return "ID: " + ID + "\nName: " + StudentUserName;
        }

    }
}