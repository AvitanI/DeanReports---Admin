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

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowNewRequests()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Request> requestListModel = bl.GetNonConfirmedRequests();
            RequestListViewModel requestListVM = new RequestListViewModel();
            List<RequestViewModel> rvm = new List<RequestViewModel>();
            foreach (Request request in requestListModel)
            {
                List<CourseRequest> courseReqestList = bl.GetCourseRequestsByRequestID(request.ID);
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
                    StudentUserName = request.StudentUserName,
                    Type = request.Type,
                    Cause = request.Cause,
                    Date = request.Date,
                    CourseRequests = courseReqestListVM
                });    
            }
            requestListVM.List = rvm;
            return View("ShowNewRequests", requestListVM);
        }
        public ActionResult DeleteRequest(int requestID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            bl.RemoveRequest(requestID);
            return RedirectToAction("ShowNewRequests");
        }
        public ActionResult ConfirmRequest(int requestID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Request request = bl.GetRequestByID(requestID);
            request.ManagerSignature = true;
            request.SignatureDate = DateTime.Now;
            bl.EditRequest(request);
            return RedirectToAction("ShowNewRequests"); 
        }
        public ActionResult UpdateRequest(RequestViewModel requestViewModel)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Request request = bl.GetRequestByID(requestViewModel.ID);
            request.ApprovalHours = requestViewModel.ApprovalHours;
            request.BudgetNumber = requestViewModel.BudgetNumber;
            request.Notes = requestViewModel.Notes;
            request.ManagerUserName = Session["Username"] as string;
            request.SignatureDate = DateTime.Now;
            request.ManagerSignature = true;
            bl.EditRequest(request);
            this.SendMessage(new Messages() 
            {
                From = Session["Username"] as string,
                ToUser = request.StudentUserName,
                Subject = "request",
                Content = "test"
            });
            return RedirectToAction("ShowNewRequests");
        }
        private bool SendMessage(Messages message)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            bl.AddMessage(message);
            return false;
        }
        [Route("Admin/SomeName")]
        public string test(int x)
        {
            return x + "";
        }
    }
}