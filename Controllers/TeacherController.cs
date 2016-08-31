using DeanReports.DataAccessLayer;
using DeanReports.Filters;
using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.Controllers
{
    [TeacherFilter]
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
            RefundViewModel refundVM = new RefundViewModel();
            List<Department> departmentsModel = bl.GetAllDepartments();
            List<DepartmentViewModel> departmentsVM = Services.ConverterService.ToDepartmentsViewModel(departmentsModel);
            refundVM.DepartmentsCombo = departmentsVM;
            return View("CreateNewRefund", refundVM);
        }
        [HttpGet]
        public JsonResult GetCoursesByDepartmentID(int? id)
        {
            Debug.WriteLine((int)id);
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Course> courses = bl.GetCoursesByDepartmentID((int)id);
            List<CourseViewModel> coursesViewModel = Services.ConverterService.ToCoursesViewModel(courses);
            if (coursesViewModel.Count() > 0)
            {
                return Json(coursesViewModel, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateNewRefund(RefundViewModel refundVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            Refund refundModel = new Refund();
            refundModel.TeacherUserName = username;
            refundModel.Date = DateTime.Now;
            refundModel.DepartmentID = Convert.ToInt32(refundVM.SelectedDepartment);
            refundModel.CourseID = Convert.ToInt32(refundVM.SelectedCourses);
            refundModel.LecturerName = refundVM.LecturerName;
            refundModel.IsGrouped = refundVM.IsGrouped;
            int refundID = bl.AddRefund(refundModel);
            return RedirectToAction("CreateNewSession", new { refundID = refundID });
        }
        public ActionResult ShowRefunds()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string teacherUsername = Session["Username"] as string;
            List<Refund> refundListModel = bl.GetRefundsByMemberID(teacherUsername);


            return View("ShowRefunds", new RefundListViewModel());
        }
        public ActionResult CreateNewSession(int? refundID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Refund refund = bl.GetRefundByID((int)refundID);
            SessionViewModel sessionVM = new SessionViewModel();
            sessionVM.RefundID = (int)refundID;
            sessionVM.IsGrouped = refund.IsGrouped;
            return View("CreateNewSession", sessionVM);
        }
        [HttpPost]
        public ActionResult CreateNewSession(SessionViewModel sessionVM)
        {
            if (ModelState.IsValid)
            {
                // create session model
                sessionVM.TeacherUserName = Session["Username"] as string;
                Session sessionModel = Services.ConverterService.ToSessionModel(sessionVM);

                // hours must be at least 1
                var hours = sessionVM.SumHoursPerSession;
                if (hours < 1)
                {
                    this.SetErrorMsg("מס שעות תקין בפגישה הוא לפחות 1");
                    return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID });
                }

                // check session by refund type
                // type can be: 0 - single form, 1 - group form
                var valid = false;
                switch (Convert.ToInt32(sessionVM.IsGrouped))
                {
                    case 0:
                        Debug.WriteLine("is single");
                        valid = CheckSingleForm(sessionModel);
                        break;
                    case 1:
                        Debug.WriteLine("is multi");
                        valid = CheckGroupForm(sessionModel, sessionVM.Students);
                        break;
                    default:
                        break;
                }
                // in case single or group form is not valid
                if (!valid) { return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID }); }

                // check if teacher dont have already session in this date
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                List<Session> duplicates = bl.GetDuplicateSessions(sessionModel);
                if (duplicates.Count() > 0)
                {
                    this.SetErrorMsg(this.GetDuplicateMsg(duplicates));
                    return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID });
                }

                // add session to db
                if (sessionVM.IsGrouped)
                {
                    foreach (var item in sessionVM.Students)
                    {
                        sessionModel.StudentUserName = item;
                        bl.AddSession(sessionModel);
                    }
                }
                else
                {
                    bl.AddSession(sessionModel);
                }
                return RedirectToAction("ShowSessions");
            }
            else
            {
                this.SetErrorMsg("שדות לא תקינים");
                return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID });
            }
        }
        private bool CheckSingleForm(Session session)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());

            // must be same student user in single form
            List<Session> sessionModel = bl.GetSessionsByRefundID(session.RefundID);
            if (sessionModel.Count() > 0 &&
                sessionModel[0].StudentUserName != session.StudentUserName)
            {
                this.SetErrorMsg("טופס יחיד חייב להכיל אך ורק פגישות לאותו הסטודנט");
                return false;
            }

            // check that this student dont have already session in this date
            List<Session> duplicates = bl.GetDuplicateSessionsByStudentUsername(session);
            if (duplicates.Count() > 0)
            {
                this.SetErrorMsg(this.GetDuplicateMsg(duplicates));
                return false;
            }
            return true;
        }
        private bool CheckGroupForm(Session session, string[] students)
        {
            // must be different students in group form
            bool ans = students.GroupBy(n => n).Any(c => c.Count() > 1);
            if (ans)
            {
                this.SetErrorMsg("לא ניתן להכניס את אותו הסטודנט");
                return false;
            }

            // check for duplicats sessions by student
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            int counter = 0;
            string msg = "";
            for (int i = 0; i < students.Length; i++)
            {
                session.StudentUserName = students[i];
                List<Session> duplicates = bl.GetDuplicateSessionsByStudentUsername(session);
                if (duplicates.Count() > 0)
                {
                    msg += this.GetDuplicateMsg(duplicates);
                    counter++;
                }
            }
            if (counter > 0) { this.SetErrorMsg(msg); return false; }
            return true;
        }
        private string GetDuplicateMsg(List<Session> duplicates)
        {
            string dupStr = "נמצאה כפילות בפגישות:<div style=\"min-width:700px;\"> </br>";
            foreach (var item in duplicates)
            {
                dupStr += "מס טופס: " + item.RefundID + 
                        " מס בקשה: " + item.ID +
                        " שם חונך: " + item.TeacherUserName +
                        " שם סטודנט: " + item.StudentUserName + "<br>";
            }
            return dupStr + "</div>";
        }
        public ActionResult ShowSessions()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string teacherUsername = Session["Username"] as string;
            RefundListViewModel refundListVM = new RefundListViewModel();
            List<Refund> refundListModel = bl.GetTeacherRefunds(teacherUsername);
            List<RefundViewModel> refundVM = new List<RefundViewModel>();
            foreach (Refund refund in refundListModel)
            {
                List<SessionViewModel> sessionVM = new List<SessionViewModel>();

                foreach (Session sessions in refund.Sessions)
                {
                    sessionVM.Add(new SessionViewModel()
                    {
                        ID = sessions.ID,
                        StudentUserName = sessions.StudentUserName,
                        RefundID = sessions.RefundID,
                        TeacherUserName = sessions.TeacherUserName,
                        Date = sessions.Date,
                        StartHour = sessions.StartHour,
                        EndHour = sessions.EndHour,
                        Details = sessions.Details
                    });
                }
                refundVM.Add(new RefundViewModel() 
                {
                    ID = refund.ID,
                    TeacherUserName = refund.TeacherUserName,
                    Date = refund.Date,
                    DepartmentID = refund.DepartmentID,
                    CourseID = refund.CourseID,
                    LecturerName = refund.LecturerName,
                    RefundSessions = sessionVM,
                    IsGrouped = refund.IsGrouped
                });
            }
            refundListVM.List = refundVM;
            return View("ShowSessions", refundListVM);
        }
        public ActionResult DeleteSession(int sessionID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            bl.RemoveSession(sessionID);
            return RedirectToAction("ShowSessions");
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