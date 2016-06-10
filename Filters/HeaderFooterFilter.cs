using DeanReports.DataAccessLayer;
using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Filters
{
    public class HeaderFooterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //ViewResult v = filterContext.Result as ViewResult;
            //if (v != null) // v will null when v is not a ViewResult
            //{
            //    BaseViewModel bvm = v.Model as BaseViewModel;
            //    if (bvm != null)//bvm will be null when we want a view without Header and footer
            //    {
            //        FinalDB db = new FinalDB();
            //        //User u = db.Users.SingleOrDefault(x => x.UserName == HttpContext.Current.User.Identity.Name);
            //        //HttpContext.Current.Session.Add("Status", u.Type);
            //        //HttpContext.Current.Session.Add("LastLogin", DateTime.Now);
            //        bvm.Identity = HttpContext.Current.User.Identity.Name;
            //        //bvm.Type = ""
            //        //bvm.LastLogin = (DateTime)HttpContext.Current.Session["LastLogin"];
            //        //bvm.Type = Convert.ToString(HttpContext.Current.Session["Status"]);
            //        //string s = (string)HttpContext.Current.Session["Status"];
            //        //Debug.WriteLine("gfsdgdfgjfdgds: " + HttpContext.Current.Session["Status"]);
            //    }
            //}
        }
    }
}