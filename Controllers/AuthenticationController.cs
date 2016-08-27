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
    [AllowAnonymous]
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
                if (user != null && user.Type != Types.NonUser && user.IsActive)
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
                    return View("Login", new UserViewModel() { UserName= u.UserName});
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
            Session["Type"] = Services.Utilities.GetUserTypeName(u.Type);
            Session["Role"] = u.Type;
            Session["IsAdmin"] = (u.Type == Types.Admin) ? true : false;
            Session["LastLoginDate"] = u.LastLogin.ToString("dd/MM/yy");
            Session["LastLoginHour"] = u.LastLogin.ToString("HH:mm");
            Session["UserImg"] = u.UserImg;
            Session["FullName"] = member.LastName + " " + member.FirstName;
            Session["DepartmentID"] = member.DepartmentID;
            Session["Messages"] = Services.ConverterService.ToMessagesViewModel(bl.GetMessagesByUser(u.UserName, Services.Utilities.MessageFilter.To));
            Session["Menu"] = Services.Utilities.GetMenuByUserType(u);
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
            registerViewModel.AcademicYears = Services.Utilities.AcademicYears;
            registerViewModel.GenderArr = Services.Utilities.Genders;
            return View("Register", registerViewModel);
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            string validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
            Debug.WriteLine(validationErrors);

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
                    string imgPath = (registerViewModel.GetGender == "זכר") ? Services.Utilities.IMG_MALE_DEFAULT : Services.Utilities.IMG_MALE_DEFAULT;
                    // add new user
                    User u = new User()
                    {
                        UserName = registerViewModel.UserName,
                        Password = registerViewModel.Password,
                        LastLogin = DateTime.Now,
                        Type = Types.NonUser,
                        UserImg = imgPath,
                        CreatedDate = DateTime.Now
                    };
                    bl.AddUser(u);
                    // add new member to user
                    Member member = new Member()
                    {
                        MemberUserName = registerViewModel.UserName,
                        Identity = registerViewModel.Identity,
                        DepartmentID = registerViewModel.DepartmentID,
                        Year = (registerViewModel.SelectedYear == null) ? "Default" : Services.Utilities.AcademicYears[(int)registerViewModel.SelectedYear],
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                        Birth = DateTime.ParseExact(registerViewModel.Birth, "dd/MM/yy", null),
                        Phone = registerViewModel.Phone,
                        Gender = registerViewModel.GetGender
                    };
                    bl.AddMember(member);
                    // send confirm mail to user
                    Services.Utilities.SendEmail(registerViewModel.UserName, "ConfirmMail");
                    return RedirectToAction("Login");
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
                registerViewModel.AcademicYears = Services.Utilities.AcademicYears;
                return RedirectToAction("Register");
            }
        }
        [Authorize]
        public ActionResult UserProfile()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            string userame = Session["Username"] as string;
            UserProfile userProfileModel = bl.GetUserProfileByUsername(userame);
            Debug.WriteLine("img: " + userProfileModel.UserImg);
            UserProfileViewModel userProfileVM = new UserProfileViewModel()
            {
                Identity = userProfileModel.Identity,
                FirstName = userProfileModel.FirstName,
                LastName = userProfileModel.LastName,
                DepartmentID = (userProfileModel.DepartmentID != null) ? userProfileModel.DepartmentID : 1,
                Year = userProfileModel.Year,
                Gender = userProfileModel.Gender,
                Birth = userProfileModel.Birth.ToString("dd/MM/yyyy"),
                Phone = userProfileModel.Phone,
                UserName = userProfileModel.UserName,
                Password = userProfileModel.Password,
                Type = userProfileModel.Type,
                LastLogin = userProfileModel.LastLogin,
                UserImg = userProfileModel.UserImg,
                UserProfileImages = (userProfileModel.Gender == "זכר") ? Services.Utilities.maleImgs : Services.Utilities.femaleImgs
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
            string departName = (from d in departmentViewModelList
                            where userProfileVM.DepartmentID == d.ID
                            select d.Name).Single();

            userProfileVM.IsStudent = (departName == "אחר") ? false : true;
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
                    Password = userProfileVM.Password,
                    UserImg = userProfileVM.UserImg,
                    IsActive = true,
                    Type = (Types)Session["Role"]

                };
                bl.EditUser(userModel);

                Member memberModel = new Member()
                {
                    MemberUserName = userProfileVM.UserName,
                    Identity = userProfileVM.Identity,
                    DepartmentID = (userProfileVM.DepartmentID != null) ? userProfileVM.DepartmentID : 1,
                    Year = (String.IsNullOrEmpty(userProfileVM.Year)) ? "Default" : userProfileVM.Year,
                    Gender = userProfileVM.Gender,
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
        public ActionResult SendUserDetails(string username)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            User u = bl.GetUserByUsername(username);
            if (u == null) return View("Login");
            Services.Utilities.SendEmail(username, "SendUserDetails", "Username: " + u.UserName + "</br>" + "Password: " + u.Password);
            return View("Login", new UserViewModel() { UserName = username });
        }
        public string test()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            return bl.AddUser(new User() { UserName="test1234@gmail.com", Password="1234", LastLogin=DateTime.Now, Type=Types.Admin}) + "";
            //return "successe";
        }
    }
}