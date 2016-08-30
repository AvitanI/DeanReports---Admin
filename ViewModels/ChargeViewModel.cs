using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class ChargeViewModel
    {
        public int ID { get; set; }
        public string StudentUserName { get; set; }
        public string TeacherUserName { get; set; }
        public DateTime Period { get; set; }
        public double CostPerHour { get; set; }
        public int SumOfHours { get; set; }
        public double SumOfBill
        {
            get { return CostPerHour * SumOfHours; }
            private set { }
        }
        public string Notes { get; set; }
        public DateTime Date { get; set; }

        // internal use

        public string ManagerUserName { get; set; }
        public int? BudgetNumber { get; set; }
        public string FundSource { get; set; }
        public bool? ManagerSignature { get; set; }
        public DateTime? SignatureDate { get; set; }
    }
}