using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string Identity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Year { get; set; }
        public string Birth { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Types Type { get; set; }
        public DateTime LastLogin { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
    }
}