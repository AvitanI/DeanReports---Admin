using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Globalization;
using System.Web.Mvc;

namespace DeanReports.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(9), RegularExpression(@"^\d{4,9}$")]
        public string Identity { get; set; }
        [Required, MaxLength(30)]
        public string FirstName { get; set; }
        [Required, MaxLength(30)]
        public string LastName { get; set; }
        [Required, RegularExpression(@"^\d+$")]
        public int? DepartmentID { get; set; }
        [Required, Range(0, 4)]
        public int SelectedYear { get; set; }
        public string Year { get; set; }
        [Required, RegularExpression(@"^\d{2}\/\d{2}\/\d{4}$")]
        public string Birth { get; set; }
        // only 048110550 or 0525642144
        [Required, MaxLength(10), RegularExpression(@"^\+?(972|0)(\-)?0?(([23489]{1}\d{7})|[5]{1}\d{8})$")]
        public string Phone { get; set; }
        [Required, EmailAddress, RegularExpression(@"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$")]
        public string UserName { get; set; }
        [Required, MinLength(4), MaxLength(20)]
        public string Password { get; set; }
        public List<DepartmentViewModel> DepartmentList { get; set; }
        public List<ProgramsViewModel> Programs { get; set; }
        public int[] SelectedPrograms { get; set; }
        public string[] AcademicYears { get; set; }
    }
}