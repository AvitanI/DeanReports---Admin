using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [NotMapped]
    public class RefundReport
    {
        public int ID { get; set; }
        public string TeacherUserName { get; set; }
        public string TeacherFullName { get; set; }
        public DateTime Date { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string DepartmentName { get; set; }
        public string LecturerName { get; set; }
        public bool IsGrouped { get; set; }
        public string TotalHours { get; set; }

        // internal use

        public string ManagerUserName { get; set; }
        public string ManagerFullName { get; set; }
        public int BudgetNumber { get; set; }
        public string SourceFund { get; set; }
        public bool ManagerSignature { get; set; }
        public DateTime SignatureDate { get; set; }
    }
}