using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Refund")]
    public class Refund
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, ForeignKey("Member"), Column(Order = 1)]
        public string TeacherUserName { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [ForeignKey("Department"), Column(Order = 4)]
        public int DepartmentID { get; set; }
        [ForeignKey("Course"), Column(Order = 5)]
        public int CourseID { get; set; }
        [Required, StringLength(50)]
        public string LecturerName { get; set; }
        public bool IsGrouped { get; set; }

        // internal use

        [ForeignKey("Manager")]
        public string ManagerUserName { get; set; }
        public int? BudgetNumber { get; set; }
        //public string SourceFund { get; set; }
        //public bool? ManagerSignature { get; set; }
        //public DateTime? SignatureDate { get; set; }

        // navigation properties

        public virtual Manager Manager { get; set; }
        public virtual Member Member { get; set; }
        public virtual Course Course { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}