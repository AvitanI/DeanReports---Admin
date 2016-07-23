using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class SessionViewModel
    {
        public int ID { get; set; }
        public string StudentUserName { get; set; }
        public int RefundID { get; set; }
        public string TeacherUserName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public int SumHoursPerSession { get { return DateTime.Parse(EndHour.ToString()).Subtract(DateTime.Parse(StartHour.ToString())).Hours ; } }
        public string Details { get; set; }
        public bool StudentSignature { get; set; }
    }
}