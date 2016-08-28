using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [NotMapped]
    public class TeacherStatistics
    {
        public string NumOfTeachedStudents { get; set; }
        public string NumOfHours { get; set; }
        public string NumOfApprovalSessions { get; set; }
        public string NumOfSingleRefunds { get; set; }
    }
}