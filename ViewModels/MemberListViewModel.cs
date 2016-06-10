using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class MemberListViewModel : BaseViewModel
    {
        public List<MemberViewModel> List { get; set; }
    }
}