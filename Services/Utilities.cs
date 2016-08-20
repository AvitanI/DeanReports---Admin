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
            new SideBarMenuViewModel(){Type=Types.Admin, ID=10, ParentID=-1, MenuItemName="צפיה בדוחות", MenuItemHref="/Admin/ShowReports", Icon="fa-table"},
            new SideBarMenuViewModel(){Type=Types.Admin, ID=11, ParentID=-1, MenuItemName="טפסי חיוב", MenuItemHref="#", Icon="fa-credit-card"},
            new SideBarMenuViewModel(){Type=Types.Admin, ID=12, ParentID=-1, MenuItemName="טפסי זיכוי", MenuItemHref="#", Icon="fa-tasks"},
            new SideBarMenuViewModel(){Type=Types.Admin, ID=13, ParentID=-1, MenuItemName="עריכת משתמשים", MenuItemHref="#", Icon="fa-user"},
            new SideBarMenuViewModel(){Type=Types.Teacher, ID=2, ParentID=-1, MenuItemName="יצירת טופס חונכות", MenuItemHref="/Teacher/CreateNewRefund", Icon="fa-edit"},
            new SideBarMenuViewModel(){Type=Types.Teacher, ID=4, ParentID=-1, MenuItemName="צפיה בפגישות", MenuItemHref="/Teacher/ShowSessions", Icon="fa-th"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=5, ParentID=-1, MenuItemName="צפיה בבקשות", MenuItemHref="/Student/ShowRequests", Icon="fa-th"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=6, ParentID=-1, MenuItemName="הגשת בקשות", MenuItemHref="#", Icon="fa-files-o"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=7, ParentID=6, MenuItemName="כללי", MenuItemHref="/Student/CreateNewRequest?FormType=General", Icon="fa-circle-o"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=8, ParentID=6, MenuItemName="אנגלית", MenuItemHref="/Student/CreateNewRequest?FormType=English", Icon="fa-circle-o"},
            new SideBarMenuViewModel(){Type=Types.Student, ID=9, ParentID=-1, MenuItemName="צפיה בפגישות", MenuItemHref="/Student/ShowSessions", Icon="fa-th"}
        };

        public static string[] maleImgs = { "/Content/images/avatars/boy1.png", 
                                    "/Content/images/avatars/boy2.png", 
                                    "/Content/images/avatars/boy3.png" };

        public static string[] femaleImgs = { "/Content/images/avatars/girl1.png", 
                                    "/Content/images/avatars/girl2.png", 
                                    "/Content/images/avatars/girl3.png" };

        public static string[] AcademicYears = {"מכינה", "א", "ב", "ג", "ד"};
        public static string[] Genders = { "זכר", "נקבה", "אחר" };
        public static string IMG_MALE_DEFAULT = "/Content/images/avatars/boy1.png";
        public static string IMG_FEMALE_DEFAULT = "/Content/images/avatars/girl1.png";

        //public string[]  jhiophpouihpohpopi

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUserTypeName(Types type)
        {
            string t = "אינו משתמש";

            if(type == Types.Admin)
            {
                t = "מנהל";
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
        public static string GetTextWithoutHTML(string htmlContents)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContents);
            if (doc == null) return null;

            string output = "";
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                output += node.InnerText;
            }
            return output;
        }
        public static DateTime? ValidateDate(int year, int month)
        {
            if((year >= 2014 && year <= DateTime.Now.Year) && 
                    (month >= 1 && month <= 12))
            {
                return new DateTime(year, month, 1); // set day to 1 always
            }
            return null;
        }

    }
}