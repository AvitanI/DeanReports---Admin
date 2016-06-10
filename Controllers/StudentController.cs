using DeanReports.Models;
using DeanReports.ViewModels;
using DeanReports.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Controllers
{
    
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewRequest()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Programs> programs = bl.GetAllPrograms();
            List<ProgramsViewModel> programsViewModel = new List<ProgramsViewModel>();
            //ProgramsListViewModel programsListViewModel = new ProgramsListViewModel();
            RequestViewModel requestViewModel = new RequestViewModel();
            foreach (Programs p in programs)
            {
                programsViewModel.Add(new ProgramsViewModel()
                {
                    ID = p.ID,
                    Name = p.Name
                });
            }
            // need to be dynamic
            int departmentID = 1;
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
            requestViewModel.ProgramsCombo = programsViewModel;
            requestViewModel.CoursesCombo = coursesViewModel;
            return View("CreateNewRequest", requestViewModel);
        }

        [HttpPost]
        public string CreateNewRequest(RequestViewModel requestViewModel)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string userName = Session["Username"] as string;
            int requestID = bl.AddRequest(new Request()
            {
                StudentUserName = userName,
                Type = requestViewModel.Type,
                Cause = requestViewModel.Cause,
                Date = DateTime.Now
            });
            if (requestID != -1)
            {
                // add each course to the specific request
                for (int i = 0; i < requestViewModel.SelectedCourses.Length; i++)
                {
                    int courseID = 0;
                    if (int.TryParse(requestViewModel.SelectedCourses[i], out courseID))
                    {
                        bl.AddCourseRequest(new CourseRequest
                        {
                            RequestID = requestID,
                            StudentUserName = userName,
                            CourseID = courseID,
                            LecturerName = requestViewModel.LecturerName[i]
                        });
                    }
                }
            }
            else { return "error"; }
            return "success: " + requestID + " " + userName;
        }
    }
}