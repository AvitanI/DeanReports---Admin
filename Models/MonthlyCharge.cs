using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [NotMapped]
    public class MonthlyCharge
    {
        public string TeacherUserName { get; set; }
        public int SumOfHours { get; set; }
    }
}