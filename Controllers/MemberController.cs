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

namespace DeanReports.Controllers
{
    //[Authorize]
    [AllowAnonymous]
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
                membersViewModel.Add(Utilities.ConvertToMemberViewModel(member));
            }
            memberListVm.List = membersViewModel;
            return View("GetAllMembers", memberListVm);
        }
    }
}