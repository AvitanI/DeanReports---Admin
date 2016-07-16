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
        public string FormType { get; set; }
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
        public string[] CauseCombo { get; set; }
        public string[] SelectedPrograms { get; set; }
        public string[] SelectedCourses { get; set; }
        public int SelectedCause { get; set; }
        public string[] LecturerName { get; set; }
        public string[] englishCauseCombo = { "חזרה על קורס", "הפנייה של מרצה", "מניעת כישלון בקורס", "אחר" };
        public string[] generalCauseCombo = { "קושי בלימודים", "סיבה אישית", "בעיה רפואית", "מילואים", "אחר" };
        public List<CourseViewModel> englishCourseLevel = new List<CourseViewModel> {      new CourseViewModel() { ID = 1, Name = "מתקדמים 1" }, 
                                                                                            new CourseViewModel() { ID = 2, Name = "מתקדמים 2" },
                                                                                            new CourseViewModel() { ID = 3, Name = "טרום" }};
        public string GetCauseName 
        { 
            get {
                    if(this.FormType == "Englisg") {
                        return englishCauseCombo[this.SelectedCause];
                    }
                    else {
                        return generalCauseCombo[this.SelectedCause];
                    }
                } 
        }
    }
}