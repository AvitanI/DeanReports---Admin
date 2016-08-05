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
                Phone = model.Phone
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
    }
}