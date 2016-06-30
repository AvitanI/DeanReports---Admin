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
        public ActionResult CreateNewRequest(RequestViewModel requestViewModel)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string userName = Session["Username"] as string;
            int requestID = bl.AddRequest(new Request()
            {
                StudentUserName = userName,
                Type = "סוג כלשהו",
                Cause = "סיבה כלשהי",
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
            return Redirect("ShowRequests");
        }
        public ActionResult ShowRequests()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            List<Request> requestListModel = bl.GetRequestsByMemberID(username);
            RequestListViewModel requestListVM = new RequestListViewModel();
            List<RequestViewModel> rvm = new List<RequestViewModel>();
            foreach (Request request in requestListModel)
            {
                List<CourseRequest> courseReqestList = bl.GetCourseRequestsByRequestID(request.ID, username);
                List<CourseRequestViewModel> courseReqestListVM = new List<CourseRequestViewModel>();
                foreach (CourseRequest cr in courseReqestList)
	            {
		            courseReqestListVM.Add(new CourseRequestViewModel()
                    {
                        CourseID = cr.CourseID,
                        LecturerName = cr.LecturerName
                    });
	            }
                rvm.Add(new RequestViewModel()
                {
                    ID = request.ID,
                    Type = request.Type,
                    Cause = request.Cause,
                    Date = request.Date,
                    CourseRequests = courseReqestListVM
                });
            }
            requestListVM.List = rvm;
            return View("ShowRequests", requestListVM);
        }
    
    }
}
