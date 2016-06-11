using DeanReports.DataAccessLayer;
using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public string Test()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            UserProfile vm = bl.GetUserProfileByUsername("bogi@bogi.com");
            return vm.Identity + "||" + vm.FirstName + "||" + vm.Password;
        }

        public string koko()
        {
            return "koko";
        }
    }
}