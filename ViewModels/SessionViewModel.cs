using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class SessionViewModel
    {
        public int ID { get; set; }
        [Required, EmailAddress]
        public string StudentUserName { get; set; }
        [Required]
        public int RefundID { get; set; }
        public string TeacherUserName { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartHour { get; set; }
        [Required]
        public TimeSpan EndHour { get; set; }
        public int SumHoursPerSession { get { return DateTime.Parse(EndHour.ToString()).Subtract(DateTime.Parse(StartHour.ToString())).Hours ; } }
        public string Details { get; set; }
        public bool StudentSignature { get; set; }
    }
}