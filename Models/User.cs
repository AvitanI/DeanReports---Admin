using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    public enum Types { Admin, Teacher, Student, NonUser }

    [Table("User")]
    public class User
    {
        [Key, StringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address")]
        public string UserName { get; set; }
        [Required, StringLength(50)]
        public string Password { get; set; }
        [Required]
        public Types Type { get; set; }
        [Required]
        public DateTime LastLogin { get; set; }
    }
}