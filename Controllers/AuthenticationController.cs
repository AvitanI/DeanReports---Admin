using DeanReports.Models;
using DeanReports.DataAccessLayer;
using DeanReports.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;
using System.Linq;
using Postal;
using System;
using DeanReports.Filters;

namespace DeanReports.Controllers
{
    public class AuthenticationController : Controller
    {   
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login", new UserViewModel() {});
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserViewModel u)
        {
            //Debug.WriteLine("ModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                User user = bl.GetUserValidity(new User() { UserName = u.UserName, Password = u.Password });
                if (user != null && user.Type != Types.NonUser)
                {
                    FormsAuthentication.SetAuthCookie(u.UserName, false);
                    this.CreateMemberShipByUser(user);
                    bl.UpdateLastLogin(new User() { UserName = u.UserName, LastLogin = DateTime.Now });
                    return RedirectToAction("UserProfile");
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
        private void CreateMemberShipByUser(User u)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Member member = bl.GetMemberByUsername(u.UserName);
            Session["Username"] = u.UserName;
            Session["Type"] = Utilities.GetUserTypeName(u.Type);
            Session["Role"] = u.Type;
            Session["LastLoginDate"] = u.LastLogin.ToString("dd/MM/yy");
            Session["LastLoginHour"] = u.LastLogin.ToString("HH:mm");
            Session["FullName"] = member.FirstName + " " + member.LastName;
            Session["DepartmentID"] = member.DepartmentID;
            Session["Messages"] = bl.GetMessagesByUser(u.UserName); 
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
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
            List<ProgramsViewModel> programsVM = new List<ProgramsViewModel>();
            List<Programs> programsModel = bl.GetAllPrograms();
            foreach (Programs p in programsModel)
            {
                programsVM.Add(new ProgramsViewModel() 
                {
                    ID = p.ID,
                    Name = p.Name
                });
            }
            registerViewModel.DepartmentList = departmentViewModelList;
            registerViewModel.Programs = programsVM;
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

                    dynamic email = new Email("Example");
                    email.To = "Avitanidan@gmail.com";
                    email.Send();

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
        [Authorize]
        //[AdminFilter]
        public ActionResult UserProfile()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string userame = Session["Username"] as string;
            UserProfile userProfileModel = bl.GetUserProfileByUsername(userame);
            UserProfileViewModel userProfileVM = new UserProfileViewModel()
            {
                Identity = userProfileModel.Identity,
                FirstName = userProfileModel.FirstName,
                LastName = userProfileModel.LastName,
                DepartmentID = userProfileModel.DepartmentID,
                Year = userProfileModel.Year,
                Birth = userProfileModel.Birth.ToString("dd/MM/yyyy"),
                Phone = userProfileModel.Phone,
                UserName = userProfileModel.UserName,
                Password = userProfileModel.Password,
                Type = userProfileModel.Type,
                LastLogin = userProfileModel.LastLogin
            };
            List<Department> departments = bl.GetAllDepartments();
            List<DepartmentViewModel> departmentViewModelList = new List<DepartmentViewModel>();
            foreach (Department item in departments)
            {
                departmentViewModelList.Add(new DepartmentViewModel()
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            //userProfileVM.DepartmentName = (from a in departments
            //                               where a.ID == userProfileModel.DepartmentID
            //                               select a.Name).Single();
            userProfileVM.Departments = departmentViewModelList;
            return View("ShowUserProfile", userProfileVM);
        }
        [HttpPost]
        public ActionResult UpdateUserProfile(UserProfileViewModel userProfileVM)
        {
            FancyBox fb;
            if (ModelState.IsValid)
            {
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                if (Session["Username"] as string != userProfileVM.UserName && bl.IsUserExist(userProfileVM.UserName))
                {
                    fb = new FancyBox()
                    {
                        Valid = false,
                        Message = "שם המשתמש קיים כבר במערכת"
                    };
                    TempData["FancyBox"] = fb;
                    return RedirectToAction("UserProfile");
                }
                User userModel = new User()
                {
                    UserName = userProfileVM.UserName,
                    Password = userProfileVM.Password
                };
                bl.EditUser(userModel);
                Member memberModel = new Member()
                {
                    MemberUserName = userProfileVM.UserName,
                    Identity = userProfileVM.Identity,
                    DepartmentID = userProfileVM.DepartmentID,
                    Year = userProfileVM.Year,
                    FirstName = userProfileVM.FirstName,
                    LastName = userProfileVM.LastName,
                    Birth = Convert.ToDateTime(userProfileVM.Birth),
                    Phone = userProfileVM.Phone
                };
                bl.EditMember(memberModel);

                fb = new FancyBox()
                {
                    Valid = false,
                    Message = "הפרופיל עודכן בהצלחה"
                };
                TempData["FancyBox"] = fb;
                return RedirectToAction("UserProfile");
            }
            else
            {
                fb = new FancyBox()
                {
                    Valid = false,
                    Message = "שדות לא תקינים"
                };
                TempData["FancyBox"] = fb;
                return RedirectToAction("UserProfile");
            }
        }
        [HttpGet]
        public JsonResult IsUserExist(string username)
        {
            return Json(new { answer = new BussinesLayer(new FinalDB()).IsUserExist(username) }, JsonRequestBehavior.AllowGet);
        }
    }
}