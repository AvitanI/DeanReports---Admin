using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Filters
{
    public class StudentFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Debug.WriteLine((Types)filterContext.HttpContext.Session["Role"]);
            if (filterContext.HttpContext.Session["Role"] != null &&
                (Types)filterContext.HttpContext.Session["Role"] != Types.Student)
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<h1 style='color:red; text-align:center;'>הרשאה רק לסטודנט</h1>"
                };
            }
        }
    }
}