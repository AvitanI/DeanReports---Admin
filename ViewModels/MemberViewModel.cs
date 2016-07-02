using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class MemberViewModel
    {
        public string MemberUserName { get; set; }
        public string Identity { get; set; }
        public int? DepartmentID { get; set; }
        public string Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public string Phone { get; set; }
    }
}