using DeanReports.DataAccessLayer;
using DeanReports.Filters;
using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost]
        public ActionResult UpdateRequest(RequestViewModel requestViewModel)
        {
            if (ModelState.IsValid)
            {
                // get existing request
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                Request request = bl.GetRequestByID(requestViewModel.ID);
                request.ApprovalHours = requestViewModel.ApprovalHours;
                request.BudgetNumber = requestViewModel.BudgetNumber;
                request.Notes = requestViewModel.Notes;
                request.ManagerUserName = Session["Username"] as string;
                request.TeacherUserName = requestViewModel.TeacherUserName;
                request.SignatureDate = DateTime.Now;
                request.ManagerSignature = requestViewModel.ManagerSignature;
                // update request
                bl.EditRequest(request);
                // send message to student
                var memberController = DependencyResolver.Current.GetService<MemberController>();
                var sigTxt = (requestViewModel.ManagerSignature == true) ? "מאושרת" : "לא מאושרת";
                memberController.SendMessage(new Messages()
                {
                    From = Session["Username"] as string,
                    ToUser = request.StudentUserName,
                    Type = MessageType.Request,
                    Subject = "request",
                    Content = "סטטוס בקשה: " + sigTxt + " " + requestViewModel.Notes,
                    Date = DateTime.Now,
                    IsSeen = false
                });
                return RedirectToAction("ShowNewRequests");
            }
            else
            {
                this.SetErrorMsg("שדות לא תקינים");
                return RedirectToAction("ShowNewRequests");
            }
        }
        
        [Route("Admin/SomeName")]
        public string test(int x)
        {
            return x + "";
        }
        [NonAction]
        public void SetErrorMsg(string msg)
        {
            FancyBox fb = new FancyBox()
            {
                Valid = false,
                Message = msg
            };
            TempData["FancyBox"] = fb;
        }
    }
}