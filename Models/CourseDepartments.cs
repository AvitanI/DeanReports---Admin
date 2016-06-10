using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("CourseDepartments")]
    public class CourseDepartments
    {
        [Key, Column(Order = 0)]
        public int DepartmentID { get; set; }
        [Key, Column(Order = 1)]
        public int CourseID { get; set; }

        // navigation properties

        public virtual Department Department { get; set; }
        public virtual Course Course { get; set; }
    }
}