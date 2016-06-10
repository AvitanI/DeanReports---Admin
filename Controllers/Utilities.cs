using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.Controllers
{
    public class Utilities
    {
        public static MemberViewModel ConvertToMemberViewModel(Member m)
        {
            return new MemberViewModel()
            {
                MemberUserName = m.MemberUserName,
                DepartmentID = m.DepartmentID,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Birth = m.Birth,
                Phone = m.Phone
            };
        }   
    }
}