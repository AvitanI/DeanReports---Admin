﻿using DeanReports.Models;
using System.Web.Mvc;

namespace DeanReports.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Debug.WriteLine((Types)filterContext.HttpContext.Session["Role"]);
            if (filterContext.HttpContext.Session["Role"] != null &&
                (Types)filterContext.HttpContext.Session["Role"] != Types.Admin)
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "<h1 style='color:red; text-align:center;'>הרשאה רק לאדמין</h1>"
                };
            }
        }
    }
}