using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Session")]
    public class Session
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, ForeignKey("Member"), Column(Order = 1)]
        public string StudentUserName { get; set; }
        [ForeignKey("Refund"), Column(Order = 2)]
        public int RefundID { get; set; }
        [ForeignKey("Refund"), Column(Order = 3)]
        public string TeacherUserName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartHour { get; set; }
        [Required]
        public TimeSpan EndHour { get; set; }
        [Required]
        public int SumHoursPerSession { get; set; }
        //return DateTime.Parse(EndHour.ToString()).Subtract(DateTime.Parse(StartHour.ToString())).Hours; 
        //public int SumHoursPerSession { get; set; }
        [StringLength(250)]
        public string Details { get; set; }
        public bool? StudentSignature { get; set; }

        // navigation properties

        public virtual Refund Refund { get; set; }
        public virtual Member Member { get; set; }
    }
}