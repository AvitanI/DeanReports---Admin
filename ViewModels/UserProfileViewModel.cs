﻿using DeanReports.Models;
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
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int? DepartmentID { get; set; }
        [Required]
        public string Gender { get; set; }
        public string DepartmentName { get; set; }
        public string Year { get; set; }
        [Required]
        public string Birth { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public Types Type { get; set; }
        public string[] UserTypesNames { get { return new string[] {"אינו משתמש", "חניך", "חונך", "מנהל" }; } }
        public int[] UserTypes { get { return new int[]{0, 1, 2, 3};} }
        public DateTime LastLogin { get; set; }
        [Required]
        public string UserImg { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public string[] UserProfileImages { get; set; }
        public bool IsActive { get; set; }
        public bool IsStudent { get; set; }
    }
}