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
        public static string GetUserTypeName(Types type)
        {
            string t = "אינו משתמש";

            if(type == Types.Admin)
            {
                t = "אדמין";
            }
            else if(type == Types.Teacher)
            {
                t = "חונך";
            }
            else if (type == Types.Student)
            {
                t = "חניך";
            }
            return t;
        }
    }
}