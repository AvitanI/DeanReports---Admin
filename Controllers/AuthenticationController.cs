using DeanReports.Models;
using DeanReports.DataAccessLayer;
using DeanReports.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;

namespace DeanReports.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        public ActionResult Login()
        {
            return View("Login", new UserViewModel() {});
        }
        [HttpPost]
        public ActionResult Login(UserViewModel u)
        {
            //Debug.WriteLine("ModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                User user = bl.GetUserValidity(new User() { UserName = u.UserName, Password = u.Password });
                if (user != null && user.Type != Types.NonUser)
                {
                    Member member = bl.GetMemberByUsername(u.UserName);
                    //FormsAuthentication.SetAuthCookie(u.UserName, false);
                    Session["Username"] = u.UserName;
                    Session["Type"] = Utilities.GetUserTypeName(user.Type);
                    Session["LastLoginDate"] = user.LastLogin.ToString("dd/MM/yy");
                    Session["LastLoginHour"] = user.LastLogin.ToString("HH:mm");
                    Session["FullName"] = member.FirstName + " " + member.LastName;
                    Session["DepartmentID"] = member.DepartmentID;
                    return RedirectToAction("GetAllMembers", "Member");
                }
                else
                {
                    FancyBox fb = new FancyBox()
                    {
                        Valid = false,
                        Message = "שם משתמש ו/או סיסמה לא תקינים"
                    };
                    TempData["FancyBox"] = fb;
                    return View("Login", new UserViewModel() { });
                }
            }
            else
            {
                FancyBox fb = new FancyBox()
                {
                    Valid = false,
                    Message = "שדות לא תקינים"
                };
                TempData["FancyBox"] = fb;
                return View("Login", new UserViewModel() { });
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Department> departments = bl.GetAllDepartments();
            RegisterViewModel registerViewModel = new RegisterViewModel();
            List<DepartmentViewModel> departmentViewModelList = new List<DepartmentViewModel>();
            foreach (Department item in departments)
            {
                departmentViewModelList.Add(new DepartmentViewModel()
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            registerViewModel.DepartmentList = departmentViewModelList;
            return View("Register", registerViewModel);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                bool exist = bl.IsUserExist(registerViewModel.UserName);
                // in case the user is exist
                if (exist)
                {
                    FancyBox fb = new FancyBox()
                    {
                        Valid = false,
                        Message = "שם משתמש זה קיים כבר במערכת"
                    };
                    TempData["FancyBox"] = fb;
                    return RedirectToAction("Register", "Authentication");
                }
                else
                {
                    // add new user
                    User u = new User()
                    {
                        UserName = registerViewModel.UserName,
                        Password = registerViewModel.Password,
                        LastLogin = System.DateTime.Now,
                        Type = Types.Admin
                    };
                    bl.AddUser(u);
                    // add new member to user
                    Member member = new Member()
                    {
                        MemberUserName = registerViewModel.UserName,
                        Identity = registerViewModel.Identity,
                        DepartmentID = registerViewModel.DepartmentID,
                        Year = registerViewModel.Year,
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                        Birth = registerViewModel.Birth,
                        Phone = registerViewModel.Phone
                    };
                    bl.AddMember(member);
                    return RedirectToAction("GetAllMembers", "Member");
                }
            }
            else
            {
                FancyBox fb = new FancyBox()
                {
                    Valid = false,
                    Message = "שדות לא תקינים"
                };
                TempData["FancyBox"] = fb;
                return RedirectToAction("Register", "Authentication");
            }
        }
        public ActionResult UserProfile()
        {
            return View("ShowUserProfile");
        }
    }
}