using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class RequestViewModel
    {
        public int ID { get; set; }
        public string StudentUserName { get; set; }
        public string Type { get; set; }
        public string Cause { get; set; }
        public DateTime Date { get; set; }
        // internal use
        public string ManagerUserName { get; set; }
        public int? ApprovalHours { get; set; }
        public int? BudgetNumber { get; set; }
        public string Notes { get; set; }
        public bool? ManagerSignature { get; set; }
        public DateTime? SignatureDate { get; set; }
        // each request has list of courses and progrmas
        public List<CourseRequestViewModel> CourseRequests { get; set; }
        public List<ProgramsViewModel> ProgramsCombo { get; set; }
        public List<CourseViewModel> CoursesCombo { get; set; }
        public string[] SelectedPrograms { get; set; }
        public string[] SelectedCourses { get; set; }
        public string[] LecturerName { get; set; }
    }
}