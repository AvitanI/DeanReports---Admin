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
        [Display(Name = "test1")]
        public string Requests { get; set; }
        [Display(Name = "test2")]
        public string Sessions { get; set; }
        [Display(Name = "test3")]
        public string CourseRequets { get; set; }
        [Display(Name = "test4")]
        public string ApprovalHours { get; set; }
    }
}