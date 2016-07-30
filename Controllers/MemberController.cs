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
        public ActionResult ShowAllMessages()
        {
            var username = Session["Username"] as string;
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            MessagesListViewModel messagesListViewModel = new MessagesListViewModel();
            List<Messages> messagesModel = bl.GetMessagesByUser(username);
            messagesListViewModel.List = Services.ConverterService.ToMessagesViewModel(messagesModel);
            return View("ShowAllMessages", messagesListViewModel);
        }
        public string test()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            //string htmlContents = 
            return bl.GetMessagesByUser("admin@gmail.com")[0].Content;

            
        }
    }
}