using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public string Identity { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int? DepartmentID { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public DateTime Birth { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public List<DepartmentViewModel> DepartmentList { get; set; }
    }
}