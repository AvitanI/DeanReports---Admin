﻿using DeanReports.DataAccessLayer;
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
            //int departmentID = (int)Session["DepartmentID"];
            int departmentID = 1;
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
        public ActionResult CreateNewRefund(RefundViewModel refundVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            Refund refundModel = new Refund();
            refundModel.TeacherUserName = username;
            refundModel.Date = DateTime.Now;
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
            //RefundViewModel refundVM = new RefundViewModel()
            //{
            //    ID = refund.ID,
            //    TeacherUserName = refund.TeacherUserName,
            //    Date = refund.Date,
            //    CourseID = refund.CourseID,
            //    LecturerName = refund.LecturerName,
            //    Type = refund.Type
            //};
            SessionViewModel sessionVM = new SessionViewModel();
            sessionVM.RefundID = (int)refundID;
            //Debug.WriteLine("IsGrouped: " + refund.IsGrouped);
            sessionVM.IsGrouped = refund.IsGrouped;
            return View("CreateNewSession", sessionVM);
        }
        [HttpPost]
        public ActionResult CreateNewSession(SessionViewModel sessionVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            if (ModelState.IsValid)
            {
                var hours = sessionVM.SumHoursPerSession;
                if (hours < 1)
                {
                    this.SetErrorMsg("מס השעות חייב להיות לפחות 1");
                    return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID });
                }


                var valid = false;
                // zero - sibgle form, 1-multi
                Debug.WriteLine("Type: " + Convert.ToInt32(sessionVM.IsGrouped));
                switch (Convert.ToInt32(sessionVM.IsGrouped))
                {
                    case 0:
                        Debug.WriteLine("is single");
                        valid = CheckSingleForm(sessionVM);
                        break;
                    case 1:
                        Debug.WriteLine("is multi");
                        valid = CheckMultiForm(sessionVM);
                        break;
                    default:
                        break;
                }

                if (!valid) { return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID }); }

                var session = new Session()
                {
                    TeacherUserName = Session["Username"] as string,
                    Date = sessionVM.Date,
                    StartHour = sessionVM.StartHour,
                    EndHour = sessionVM.EndHour
                };
                var x = bl.GetDuplicateSessions(session).Count();
                var str = "";
                foreach (var item in bl.GetDuplicateSessions(session))
                {
                    str += item.ID + " || ";
                }
                if (x > 0)
                {
                    this.SetErrorMsg("פגישה קיימת " + str);
                    return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID });
                }

                // check for duplicates

                Session sessionModel = new Session()
                {
                    StudentUserName = sessionVM.StudentUserName,
                    RefundID = sessionVM.RefundID,
                    TeacherUserName = Session["Username"] as string,
                    Date = DateTime.Now,
                    StartHour = sessionVM.StartHour,
                    EndHour = sessionVM.EndHour,
                    SumHoursPerSession = sessionVM.SumHoursPerSession,
                    Details = sessionVM.Details,
                    StudentSignature = false
                };
                bl.AddSession(sessionModel);
                //return "success";
                return RedirectToAction("ShowSessions");
            }
            else
            {
                this.SetErrorMsg("שדות לא תקינים");
                return RedirectToAction("CreateNewSession", new { RefundID = sessionVM.RefundID});
                //return "failed";
            }
            //return RedirectToAction("ShowSessions");
        }
        private bool CheckSingleForm(SessionViewModel sessionVM)
        {
            var result = true;
            BussinesLayer bl = new BussinesLayer(new FinalDB());

            // check student
            List<Session> sessionModel = bl.GetSessionsByRefundID(sessionVM.RefundID);
            if (sessionModel.Count() > 0 && 
                sessionModel[0].StudentUserName != sessionVM.StudentUserName)
            {
                this.SetErrorMsg("לא ניתן להכניס סטודנט אחר");
                result = false;
            }
            return result;
        }
        private bool CheckMultiForm(SessionViewModel sessionVM)
        {
            var result = true;

            bool ans = sessionVM.Students.GroupBy(n => n).Any(c => c.Count() > 1);
            if (ans)
            {
                this.SetErrorMsg("לא ניתן להכניס את אותו הסטודנט");
                result = false;
            }

            //

            return result;
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
                    CourseID = refund.CourseID,
                    LecturerName = refund.LecturerName,
                    RefundSessions = sessionVM
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