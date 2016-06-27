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
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewRefund()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            int departmentID = (int)Session["DepartmentID"];
            RefundViewModel refundVM = new RefundViewModel();
            List<Course> courses = bl.GetCoursesByDepartmentID(departmentID);
            List<CourseViewModel> coursesViewModel = new List<CourseViewModel>();
            foreach (Course c in courses)
            {
                coursesViewModel.Add(new CourseViewModel()
                {
                    ID = c.ID,
                    Name = c.Name
                });
            }
            refundVM.CoursesCombo = coursesViewModel;
            return View("CreateNewRefund", refundVM);
        }
        [HttpPost]
        public string CreateNewRefund(RefundViewModel refundVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            Refund refundModel = new Refund();
            refundModel.TeacherUserName = username;
            refundModel.Date = DateTime.Now;
            refundModel.CourseID = Convert.ToInt32(refundVM.SelectedCourses);
            refundModel.LecturerName = refundVM.LecturerName;
            bl.AddRefund(refundModel);
            //return View("ShowRefunds");
            return refundVM.LecturerName + "";
        }

        public ActionResult CreateNewSession()
        {
            return View("CreateNewSession");
        }


    }
}