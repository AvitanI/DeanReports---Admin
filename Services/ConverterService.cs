using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.Services
{
    public class ConverterService
    {
        public static List<ProgramsViewModel> ToProgramsViewModel(List<Programs> model)
        {
            List<ProgramsViewModel> viewModel = new List<ProgramsViewModel>();
            foreach (Programs p in model)
            {
                viewModel.Add(new ProgramsViewModel()
                {
                    ID = p.ID,
                    Name = p.Name
                });
            }
            return viewModel;
        }
        public static List<CourseViewModel> ToCoursesViewModel(List<Course> model)
        {
            List<CourseViewModel> viewModel = new List<CourseViewModel>();
            foreach (Course c in model)
            {
                viewModel.Add(new CourseViewModel()
                {
                    ID = c.ID,
                    Name = c.Name
                });
            }
            return viewModel;
        }
        public static List<DepartmentViewModel> ToDepartmentsViewModel(List<Department> model)
        {
            List<DepartmentViewModel> viewModel = new List<DepartmentViewModel>();
            foreach (Department d in model)
            {
                viewModel.Add(new DepartmentViewModel()
                {
                    ID = d.ID,
                    Name = d.Name
                });
            }
            return viewModel;
        }
        public static List<CourseRequestViewModel> ToCourseRequestViewModel(List<CourseRequest> model)
        {
            List<CourseRequestViewModel> viewModel = new List<CourseRequestViewModel>();
            foreach (CourseRequest cr in model)
            {
                viewModel.Add(new CourseRequestViewModel()
                {
                    CourseID = cr.CourseID,
                    LecturerName = cr.LecturerName
                });
            }
            return viewModel;
        }
        public static RequestViewModel ToRequestViewModel(Request model, List<CourseRequest> list)
        {
            return new RequestViewModel() 
            {
                ID = model.ID,
                Type = model.Type,
                Cause = model.Cause,
                Date = model.Date,
                CourseRequests = ToCourseRequestViewModel(list)
            };
        }
        public static MemberViewModel ToMemberViewModel(Member model)
        {
            return new MemberViewModel()
            {
                MemberUserName = model.MemberUserName,
                DepartmentID = model.DepartmentID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birth = model.Birth,
                Phone = model.Phone,
                Gender = model.Gender
            };
        }
        public static List<SessionViewModel> ToSessionViewModel(List<Session> model)
        {
            List<SessionViewModel> viewModel = new List<SessionViewModel>();
            foreach (Session s in model)
            {
                viewModel.Add(new SessionViewModel()
                {
                    ID = s.ID,
                    StudentUserName = s.StudentUserName,
                    RefundID = s.RefundID,
                    TeacherUserName = s.TeacherUserName,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour,
                    Details = s.Details,
                    StudentSignature = s.StudentSignature
                });
            }
            return viewModel;
        }
        public static Messages ToMessagesModel(MessagesViewModel viewModel)
        {
            return new Messages()
            {
                From = viewModel.From,
                ToUser = viewModel.ToUser,
                Subject = viewModel.Subject,
                Content = viewModel.Content
            };
        }
        public static List<MessagesViewModel> ToMessagesViewModel(List<Messages> model)
        {
            List<MessagesViewModel> viewModel = new List<MessagesViewModel>();
            foreach (Messages m in model)
            {
                viewModel.Add(new MessagesViewModel()
                {
                    ID = m.ID,
                    From = m.From,
                    ToUser = m.ToUser,
                    Subject = m.Subject,
                    Content = m.Content,
                    PureContent = Services.Utilities.GetTextWithoutHTML(m.Content),
                    Date = m.Date,
                    IsSeen = m.IsSeen,
                    SeenDate = m.SeenDate
                });
            }
            return viewModel;
        }
        public static Session ToSessionModel(SessionViewModel viewModel)
        {
            return new Session()
            {
                StudentUserName = viewModel.StudentUserName,
                RefundID = viewModel.RefundID,
                TeacherUserName = viewModel.TeacherUserName,
                Date = viewModel.Date,
                StartHour = viewModel.StartHour,
                EndHour = viewModel.EndHour,
                Details = viewModel.Details,
                StudentSignature = viewModel.StudentSignature
            };
        }
        public static List<ChargeReportViewModel> ToChargeReportViewModel(List<ChargeReport> model)
        {
            List<ChargeReportViewModel> viewModel = new List<ChargeReportViewModel>();
            foreach (ChargeReport cr in model)
            {
                viewModel.Add(new ChargeReportViewModel()
                {
                    ID = cr.ID,
                    StudentID = cr.StudentID,
                    StudentFullName = cr.StudentFullName,
                    TeacherFullName = cr.TeacherFullName,
                    TeacherID = cr.TeacherID,
                    Period = cr.Period,
                    CostPerHour = cr.CostPerHour,
                    SumOfHours = cr.SumOfHours,
                    SumOfBill = cr.SumOfBill,
                    Notes = cr.Notes,
                    Date = cr.Date,
                    ManagerUserName = cr.ManagerUserName,
                    BudgetNumber = cr.BudgetNumber,
                    FundSource = cr.FundSource,
                    ManagerSignature = cr.ManagerSignature,
                    SignatureDate = cr.SignatureDate
                });
            }
            return viewModel;
        }
        public static List<RefundReportViewModel> ToRefundReportViewModel(List<RefundReport> model)
        {
            List<RefundReportViewModel> viewModel = new List<RefundReportViewModel>();
            foreach (RefundReport rr in model)
            {
                viewModel.Add(new RefundReportViewModel()
                {
                    ID = rr.ID,
                    TeacherUserName = rr.TeacherUserName,
                    TeacherFullName = rr.TeacherFullName,
                    Date = rr.Date,
                    CourseID = rr.CourseID,
                    CourseName = rr.CourseName,
                    DepartmentName = rr.DepartmentName,
                    LecturerName = rr.LecturerName,
                    IsGrouped = rr.IsGrouped,
                    TotalHours = rr.TotalHours,
                    ManagerUserName = rr.ManagerUserName,
                    ManagerFullName  = rr.ManagerFullName,
                    BudgetNumber = rr.BudgetNumber,
                    SourceFund = rr.SourceFund,
                    ManagerSignature = rr.ManagerSignature,
                    SignatureDate = rr.SignatureDate
                });
            }
            return viewModel;
        }
        public static List<UsersReportViewModel> ToUsersReportViewModel(List<UserReport> model)
        {
            List<UsersReportViewModel> viewModel = new List<UsersReportViewModel>();
            foreach (UserReport ur in model)
            {
                viewModel.Add(new UsersReportViewModel()
                {
                    UserName = ur.UserName,
                    Password = ur.Password,
                    Type = ur.Type,
                    LastLogin = ur.LastLogin,
                    UserImg = ur.UserImg,
                    CreatedDate = ur.CreatedDate,
                    IsActive = ur.IsActive,
                    MemberUserName = ur.MemberUserName,
                    Identity = ur.Identity,
                    DepartmentID = ur.DepartmentID,
                    Year = ur.Year,
                    FirstName = ur.FirstName,
                    LastName = ur.LastName,
                    Birth = ur.Birth,
                    Phone = ur.Phone,
                    Gender = ur.Gender,
                    MemberFullName = ur.MemberFullName,
                    DepartmentName = ur.DepartmentName
                });
            }
            return viewModel;
        }
    }
}