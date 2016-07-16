using DeanReports.Models;
using DeanReports.ViewModels;
using DeanReports.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeanReports.Filters;
using DeanReports.Services;

namespace DeanReports.Controllers
{
    [StudentFilter]
    public class StudentController : Controller
    {
        // class variables

        
        

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateNewRequest(string formType)
        {
            switch (formType.ToLower())
            {
                case "general":
                    return View("CreateNewRequest", LoadRequestViewModel(false));

                case "english":
                    return View("CreateNewEnglishRequest", LoadRequestViewModel(true));
                default:
                    return new EmptyResult();
            }
        }
        private RequestViewModel LoadRequestViewModel(bool isEnglish)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            RequestViewModel requestViewModel = new RequestViewModel();
            List<Programs> programs = bl.GetAllPrograms();
            requestViewModel.ProgramsCombo = ConverterService.ToProgramsViewModel(programs);
            if (!isEnglish)
            {
                int departmentID = (int)Session["DepartmentID"];
                List<Course> courses = bl.GetCoursesByDepartmentID(departmentID);
                requestViewModel.CoursesCombo = ConverterService.ToCoursesViewModel(courses);
            }
            return requestViewModel;
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
                Cause = requestViewModel.GetCauseName,
                Date = DateTime.Now,
                FormType = requestViewModel.FormType
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
                List<CourseRequest> courseReqestList = bl.GetCourseRequestsByRequestID(request.ID);
                RequestViewModel requestViewModel = ConverterService.ToRequestViewModel(request, courseReqestList);
                rvm.Add(requestViewModel);
            }
            requestListVM.List = rvm;
            return View("ShowRequests", requestListVM);
        }
        public JsonResult Test()
        {
            return Json(new {foo="bar", baz="Blech"}, JsonRequestBehavior.AllowGet);
        }
    }
}
