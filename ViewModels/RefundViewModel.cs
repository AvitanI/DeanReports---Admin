using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class RefundViewModel
    {
        public int ID { get; set; }
        public string TeacherUserName { get; set; }
        public DateTime Date { get; set; }
        public int CourseID { get; set; }
        public string SelectedCourses { get; set; }
        public string LecturerName { get; set; }
        // internal use
        public string ManagerUserName { get; set; }
        public int? BudgetNumber { get; set; }
        // each refund has list of sessions
        public List<SessionViewModel> RefundSessions { get; set; }
        public List<CourseViewModel> CoursesCombo { get; set; }
    }
}