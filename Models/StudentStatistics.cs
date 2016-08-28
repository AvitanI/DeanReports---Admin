using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [NotMapped]
    public class StudentStatistics
    {
        public string Requests { get; set; }
        public string Sessions { get; set; }
        public string CourseRequets { get; set; }
        public string ApprovalHours { get; set; }
    }
}