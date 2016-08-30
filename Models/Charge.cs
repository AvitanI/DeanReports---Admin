using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    [Table("Charge")]
    public class Charge
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Key, ForeignKey("Member"), Column(Order = 1)]
        public string StudentUserName { get; set; }
        [Key, Column(Order = 2)]
        public string TeacherUserName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Period { get; set; }
        [Required]
        public double CostPerHour { get; set; }
        [Required]
        public int SumOfHours { get; set; }
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double SumOfBill {get;set;}
        [Required, StringLength(250)]
        public string Notes { get; set; }
        [Required]
        public DateTime Date { get; set; }

        // internal use

        [ForeignKey("Manager")]
        public string ManagerUserName { get; set; }
        public int? BudgetNumber { get; set; }
        public string FundSource { get; set; }
        public bool? ManagerSignature { get; set; }
        public DateTime? SignatureDate { get; set; }

        // navigation properties

        public virtual Manager Manager { get; set; }
        public virtual Member Member { get; set; }
    }
}