using DeanReports.Models;
using System.Web.Mvc;

namespace DeanReports.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Status"] == null ||
                (Types)filterContext.HttpContext.Session["Status"] != Types.Admin)
            {
                filterContext.Result = new ContentResult()
                {
                    Content = "Unauthorized to access specified resource."
                };
            }
        }
    }
}