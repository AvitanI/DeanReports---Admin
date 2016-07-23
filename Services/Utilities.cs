using DeanReports.Models;
using DeanReports.ViewModels;
using Postal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DeanReports.Services
{
    public class Utilities
    {
        // class variables

        public static List<SideBarMenuViewModel> Menus = new List<SideBarMenuViewModel>()
        {
            new SideBarMenuViewModel(){Type=Types.Admin, ID=1, ParentID=-1, MenuItemName="בקשות חדשות", MenuItemHref="/Admin/ShowNewRequests", Icon="fa-files-o"},
            new SideBarMenuViewModel(){Type=Types.Teacher, ID=2, ParentID=-1, MenuItemName="יצירת טופס חונכות", MenuItemHref="/Teacher/CreateNewRefund", Icon="fa-edit"},
            new SideBarMenuViewModel(){Type=Types.Teacher, ID=3, ParentID=-1, MenuItemName="יצירת פגישה", MenuItemHref="/Teacher/CreateNewSession", Icon="fa-edit"},
            new SideBarMenuViewModel(){Type=Types.Teacher, ID=4, ParentID=-1, MenuItemName="צפייה בפגישות", MenuItemHref="/Teacher/ShowSessions", Icon="fa-th"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=5, ParentID=-1, MenuItemName="צפייה בבקשות", MenuItemHref="/Student/ShowRequests", Icon="fa-th"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=6, ParentID=-1, MenuItemName="הגשת בקשות", MenuItemHref="#", Icon="fa-files-o"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=7, ParentID=6, MenuItemName="כללי", MenuItemHref="/Student/CreateNewRequest?FormType=General", Icon="fa-circle-o"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=8, ParentID=6, MenuItemName="אנגלית", MenuItemHref="/Student/CreateNewRequest?FormType=English", Icon="fa-circle-o"},
        };

        public static string[] AcademicYears = {"מכינה", "א", "ב", "ג", "ד"};

        //public string[]  

        public static string GetUserTypeName(Types type)
        {
            string t = "אינו משתמש";

            if(type == Types.Admin)
            {
                t = "אדמין";
            }
            else if(type == Types.Teacher)
            {
                t = "חונך";
            }
            else if (type == Types.Student)
            {
                t = "חניך";
            }
            return t;
        }
        public static List<SideBarMenuViewModel> GetMenuByUserType(User u)
        {
            List<SideBarMenuViewModel> menu = new List<SideBarMenuViewModel>();
            switch (u.Type)
            {
                case Types.Admin:
                    menu = CreateMenuByUserType(Types.Admin);
                    break;
                case Types.Teacher:
                    menu = CreateMenuByUserType(Types.Teacher);
                    break;
                case Types.Student:
                    menu = CreateMenuByUserType(Types.Student);
                    break;
                default:
                    break;
            }
            return menu;
        }
        private static List<SideBarMenuViewModel> CreateMenuByUserType(Types type)
        {
            // filter only menu according specific user type
            var result = (  from m in Menus
                            where m.Type == type
                            select m).ToList();

            // add submenus for each menu item
            foreach (var item in result)
            {
                if (item.ParentID != -1)
                {
                    var parent = result.Find(obj => obj.ID == item.ParentID);
                    if(parent.SubMenus == null) {
                        parent.SubMenus = new List<SideBarMenuViewModel>();
                    }
                    parent.SubMenus.Add(item);
                }
            }
            return result;
        }

        public static void SendEmail(string to, string type)
        {
            dynamic email = new Email(type);
            email.To = to;
            email.Send();
        }
    }
}