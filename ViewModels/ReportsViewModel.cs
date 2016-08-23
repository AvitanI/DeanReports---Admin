using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class ReportsViewModel
    {
        public List<ChargeReportViewModel> Charges { get; set; }
        public List<RefundReportViewModel> Refunds { get; set; }
        public List<UsersReportViewModel> Users { get; set; }
    }
}