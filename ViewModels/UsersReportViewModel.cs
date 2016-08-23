using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class UsersReportViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Types Type { get; set; }
        public DateTime LastLogin { get; set; }
        public string UserImg { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string MemberUserName { get; set; }
        public string Identity { get; set; }
        public int? DepartmentID { get; set; }
        public string Year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string MemberFullName { get; set; }
        public string DepartmentName { get; set; }
    }
}