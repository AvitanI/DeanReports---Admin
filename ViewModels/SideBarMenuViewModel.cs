using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class SideBarMenuViewModel
    {
        public int ID { get; set; }
        public int ParentID { get; set;}
        public Types Type { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemHref { get; set; }
        public string Icon { get; set; }
        public List<SideBarMenuViewModel> SubMenus { get; set; }
    }
}