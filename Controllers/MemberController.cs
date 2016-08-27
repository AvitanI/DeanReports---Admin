using DeanReports.Filters;
using DeanReports.Models;
using DeanReports.ViewModels;
using DeanReports.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeanReports.Services;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace DeanReports.Controllers
{
    //[Authorize]
    //[AllowAnonymous]
    public class MemberController : Controller
    {
        [AllowAnonymous]
        // GET: Member
        public string Index()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            bl.InsertDummyData();

            return "success";
        }
        
        //[HeaderFooterFilter]
        //[AdminFilter]
        public ActionResult GetAllMembers()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Member> members = bl.GetAllMembers();
            List<MemberViewModel> membersViewModel = new List<MemberViewModel>();
            MemberListViewModel memberListVm = new MemberListViewModel();
            foreach (Member member in members)
            {
                membersViewModel.Add(ConverterService.ToMemberViewModel(member));
            }
            memberListVm.List = membersViewModel;
            return View("GetAllMembers", memberListVm);
        }

        public ActionResult GetMemberStatistics()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            StatisticsViewModel statVM = new StatisticsViewModel();
            return View("GetMemberStatistics", statVM);
        }
        [HttpGet]
        //public JsonResult GetMemberDetails(string username)
        //{
        //    BussinesLayer bl = new BussinesLayer(new FinalDB());
        //    Member m = bl.GetMemberByUsername(username);

        //    return Json(new { firstName = m.FirstName,
        //                      lastName = m.LastName }, 
        //                JsonRequestBehavior.AllowGet);
        //}
        //--------------------------------------------------------------
        public ActionResult SendMessages()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            MessagesViewModel messagesVM = new MessagesViewModel();
            List<Member> membersModel = bl.GetAllMembers();
            List<MemberViewModel> membersVM = new List<MemberViewModel>();
            foreach (Member member in membersModel)
            {
                membersVM.Add(Services.ConverterService.ToMemberViewModel(member));
            }
            messagesVM.Members = membersVM;
            return View("SendMessages", messagesVM);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMessages(MessagesViewModel messageVM)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            messageVM.From = Session["Username"] as string;
            Messages messagesModel = Services.ConverterService.ToMessagesModel(messageVM);
            messagesModel.Type = MessageType.General;
            this.SendMessage(messagesModel);
            return Redirect("ShowAllMessages");
        }
        [NonAction]
        public bool SendMessage(Messages message)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            message.Date = DateTime.Now;
            message.IsSeen = false;
            bl.AddMessage(message);
            return false;
        }
        //[Authorize]
        public JsonResult GetMemberDetails(string query)
        {
            if (Session == null || Session["Role"] == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Member> members = bl.GetMemberByAjax(query);
            if (members.Count() > 0)
            {
                return Json(members, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowAllMessages(int type)
        {
            var username = Session["Username"] as string;
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            MessagesListViewModel messagesListViewModel = new MessagesListViewModel();
            List<Messages> messagesModel = bl.GetMessagesByUser(username, (Utilities.MessageFilter)type);
            messagesListViewModel.List = Services.ConverterService.ToMessagesViewModel(messagesModel);
            if ((Utilities.MessageFilter)type == Utilities.MessageFilter.To)
            {
                return View("ShowInMessages", messagesListViewModel);
            }
            else if ((Utilities.MessageFilter)type == Utilities.MessageFilter.From)
            {
                return View("ShowOutMessages", messagesListViewModel);
            }
            else
            {
                return new EmptyResult();
            }
        }
        public ActionResult ReadMessageByID(int? id)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Messages messageModel = bl.GetMessageByID((int)id);
            MessagesViewModel messagViewModel = new MessagesViewModel()
            {
                ID = messageModel.ID,
                From = messageModel.From,
                ToUser = messageModel.ToUser,
                Subject = messageModel.Subject,
                Content = messageModel.Content,
                Date = messageModel.Date,
                IsSeen = messageModel.IsSeen,
                SeenDate = messageModel.SeenDate,
            };
            return View("ShowMessage", messagViewModel);
        }
        public JsonResult UpdateMessages(string ids) 
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string username = Session["Username"] as string;
            bl.UpdateMessagesToSeen(username, ids, DateTime.Now);
            return Json(new { success = ids }, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowStatistics() 
        {
            string username = Session["Username"] as string;
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            StatisticsViewModel statViewModel = new StatisticsViewModel();
            List<CircleDataViewModel> circleList = new List<CircleDataViewModel>();
            StudentStatistics statistics = bl.GetStudentStatistics(username);

            circleList.Add(new CircleDataViewModel() 
            {
                ID = "quadrant1",
                Title = "בקשות שאושרו מתוך סך הבקשות",
                Value = this.GetPercent(statistics.Requests),
                MaxValue = 100
            });

            circleList.Add(new CircleDataViewModel()
            {
                ID = "quadrant2",
                Title = "פגישות שאישרתי מתוך סך הפגישות",
                Value = this.GetPercent(statistics.Sessions),
                MaxValue = 100
            });

            circleList.Add(new CircleDataViewModel()
            {
                ID = "quadrant3",
                Title = "בקשות שהוגשו מאותו הקורס",
                Value = this.GetPercent(statistics.CourseRequets),
                MaxValue = 100
            });

            circleList.Add(new CircleDataViewModel()
            {
                ID = "quadrant4",
                Title = "ניצול השעות מתוך השעות המאושרות",
                Value = this.GetPercent(statistics.ApprovalHours),
                MaxValue = 100
            });
            statViewModel.Data = circleList;
            return View("ShowStatistics", statViewModel);
        }
        private int GetPercent(string input)
        {
            string[] values = input.Split('/');
            if(input == null || values.Length < 2) return 0;
            int numerator = Int32.Parse(values[0].Trim());
            int denominator = Int32.Parse(values[1].Trim());
            return (int)Math.Floor((double)numerator / denominator * 100);
        }
        public JsonResult SerachMessages(string query)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string usernmae = Session["Username"] as string;
            List<Messages> messages = bl.GetMessagesByAjax(usernmae, query);
            if (messages.Count() > 0)
            {
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

    }
}