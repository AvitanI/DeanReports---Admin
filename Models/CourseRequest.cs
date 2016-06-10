using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("CourseRequest")]
    public class CourseRequest
    {
        [Key, ForeignKey("Request"), Column(Order = 0)]
        public int RequestID { get; set; }
        [Key, ForeignKey("Request"), Column(Order = 1)]
        public string StudentUserName { get; set; }
        [Key, Column(Order = 2)]
        public int CourseID { get; set; }
        [Required, StringLength(50)]
        public string LecturerName { get; set; }

        // navigation properties

        public virtual Request Request { get; set; }
        public virtual Course Course { get; set; }
    }
}