using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Filters
{
    public class TeacherFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Debug.WriteLine((Types)filterContext.HttpContext.Session["Role"]);
            if (filterContext.HttpContext.Session["Role"] != null &&
                (Types)filterContext.HttpContext.Session["Role"] != Types.Teacher)
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<h1 style='color:red; text-align:center;'>הרשאה רק לחונך</h1>"
                };
            }
        }
    }
}