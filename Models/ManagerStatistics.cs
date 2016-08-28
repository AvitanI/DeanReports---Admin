using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [NotMapped]
    public class ManagerStatistics
    {
        public string NumOfApprovalRequets { get; set; }
        public string NumOfMessages { get; set; }
        public string NumOfStudents { get; set; }
        public string NumOfENRequets { get; set; }
    }
}