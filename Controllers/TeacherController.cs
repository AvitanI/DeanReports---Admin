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
        public ActionResult CreateNewRefund(RefundViewModel refundVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            Refund refundModel = new Refund();
            refundModel.TeacherUserName = username;
            refundModel.Date = DateTime.Now;
            refundModel.CourseID = Convert.ToInt32(refundVM.SelectedCourses);
            refundModel.LecturerName = refundVM.LecturerName;
            int refundID = bl.AddRefund(refundModel);
            return RedirectToAction("CreateNewSession", new { refundID = refundID });
        }
        public ActionResult CreateNewSession(int refundID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Refund refund = bl.GetRefundByID(refundID);
            RefundViewModel refundVM = new RefundViewModel()
            {
                ID = refund.ID,      
                TeacherUserName = refund.TeacherUserName,
                Date = refund.Date,
                CourseID = refund.CourseID,
                LecturerName = refund.LecturerName
            };
            List<Member> students = bl.GetAllMembersByType(Types.Student);
            List<MemberViewModel> studentsVM = new List<MemberViewModel>();
            foreach (Member s in students)
            {
                studentsVM.Add(new MemberViewModel()
                {
                    MemberUserName = s.MemberUserName,
                    Identity = s.Identity,
                    DepartmentID = s.DepartmentID,
                    Year = s.Year,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Birth = s.Birth,
                    Phone = s.Phone
                });
            }
            refundVM.Students = studentsVM;
            return View("CreateNewSession", refundVM);
        }
        [HttpPost]
        public string CreateNewSession(SessionViewModel sessionVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
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
            return sessionModel.RefundID + " || " + sessionModel.StartHour + " || " + sessionModel.StudentUserName + " || " + sessionModel.SumHoursPerSession + " || " + sessionModel.TeacherUserName;
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
                        EndHour = sessions.EndHour
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


    }
}