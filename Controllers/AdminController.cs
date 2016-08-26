using DeanReports.DataAccessLayer;
using DeanReports.Filters;
using DeanReports.Models;
using DeanReports.ViewModels;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DeanReports.Controllers
{

    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowNewRequests()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            List<Request> requestListModel = bl.GetNonConfirmedRequests();
            RequestListViewModel requestListVM = new RequestListViewModel();
            List<RequestViewModel> rvm = new List<RequestViewModel>();
            foreach (Request request in requestListModel)
            {
                List<CourseRequest> courseReqestList = bl.GetCourseRequestsByRequestID(request.ID);
                List<CourseRequestViewModel> courseReqestListVM = new List<CourseRequestViewModel>();
                foreach (CourseRequest cr in courseReqestList)
                {
                    courseReqestListVM.Add(new CourseRequestViewModel()
                    {
                        CourseID = cr.CourseID,
                        LecturerName = cr.LecturerName
                    });
                }
                rvm.Add(new RequestViewModel()
                {
                    ID = request.ID,
                    StudentUserName = request.StudentUserName,
                    Type = request.Type,
                    Cause = request.Cause,
                    Date = request.Date,
                    CourseRequests = courseReqestListVM
                });    
            }
            requestListVM.List = rvm;
            return View("ShowNewRequests", requestListVM);
        }
        public ActionResult DeleteRequest(int requestID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            bl.RemoveRequest(requestID);
            return RedirectToAction("ShowNewRequests");
        }
        public ActionResult ConfirmRequest(int requestID)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            Request request = bl.GetRequestByID(requestID);
            request.ManagerSignature = true;
            request.SignatureDate = DateTime.Now;
            bl.EditRequest(request);
            return RedirectToAction("ShowNewRequests"); 
        }
        [HttpPost]
        public ActionResult UpdateRequest(RequestViewModel requestViewModel)
        {
            if (ModelState.IsValid)
            {
                // get existing request
                BussinesLayer bl = new BussinesLayer(new FinalDB());
                Request request = bl.GetRequestByID(requestViewModel.ID);
                request.ApprovalHours = requestViewModel.ApprovalHours;
                request.BudgetNumber = requestViewModel.BudgetNumber;
                request.Notes = requestViewModel.Notes;
                request.ManagerUserName = Session["Username"] as string;
                request.TeacherUserName = requestViewModel.TeacherUserName;
                request.SignatureDate = DateTime.Now;
                request.ManagerSignature = requestViewModel.ManagerSignature;
                // update request
                bl.EditRequest(request);
                // send message to student
                var memberController = DependencyResolver.Current.GetService<MemberController>();
                var sigTxt = (requestViewModel.ManagerSignature == true) ? "מאושרת" : "לא מאושרת";
                memberController.SendMessage(new Messages()
                {
                    From = Session["Username"] as string,
                    ToUser = request.StudentUserName,
                    Type = MessageType.Request,
                    Subject = "request",
                    Content = "סטטוס בקשה: " + sigTxt + " " + requestViewModel.Notes,
                    Date = DateTime.Now,
                    IsSeen = false
                });
                return RedirectToAction("ShowNewRequests");
            }
            else
            {
                this.SetErrorMsg("שדות לא תקינים");
                return RedirectToAction("ShowNewRequests");
            }
        }
        public ActionResult ShowReports()
        {
            return View("ShowReports");
        }
        public ActionResult SearchReports(SearchReportParamsViewModel searchParams)
        {
            int year = searchParams.Year;
            int month = searchParams.Month;
            DateTime? date = Services.Utilities.ValidateDate(year, month);
            ReportsViewModel vm = this.GetReportByType(searchParams.Type, date);
            return PartialView("ShowReportByType", vm);
        }
        //[Route("Admin/SomeName")]
        private ReportsViewModel GetReportByType(int type, DateTime? reportDate)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            ReportsViewModel reportsViewModel = new ReportsViewModel();

            if (type == (int)ReportType.Charge) // load charges report
            {
                List<ChargeReport> charges = bl.GetChargeRports(reportDate);
                List<ChargeReportViewModel> chargesVM = Services.ConverterService.ToChargeReportViewModel(charges);
                reportsViewModel.Type = ReportType.Charge;
                reportsViewModel.Charges = chargesVM;
            }
            else if (type == (int)ReportType.Refund) // load refunds report
            {
                List<RefundReport> refunds = bl.GetRefundRports(reportDate);
                List<RefundReportViewModel> refundsVM = Services.ConverterService.ToRefundReportViewModel(refunds);
                reportsViewModel.Type = ReportType.Refund;
                reportsViewModel.Refunds = refundsVM;
            }
            else if (type == (int)ReportType.User) // load users report
            {
                List<UserReport> users = bl.GetUsersRports(reportDate);
                List<UsersReportViewModel> usersVM = Services.ConverterService.ToUsersReportViewModel(users);
                reportsViewModel.Type = ReportType.User;
                reportsViewModel.Users = usersVM;
            }
            return reportsViewModel;
        }
        [NonAction]
        public void SetErrorMsg(string msg)
        {
            FancyBox fb = new FancyBox()
            {
                Valid = false,
                Message = msg
            };
            TempData["FancyBox"] = fb;
        }
        public ActionResult ExportToExcel(DataTable data)
        {
            // Step 1 - get the data from database
            //var data = new List<User>() { 
            //    new User(){UserName="user1", Password="1234", Type=Types.Admin, LastLogin=DateTime.Now, UserImg="c:/blabla"},
            //    new User(){UserName="user2", Password="1234", Type=Types.Admin, LastLogin=DateTime.Now, UserImg="c:/blabla"},
            //    new User(){UserName="user3", Password="1234", Type=Types.Admin, LastLogin=DateTime.Now, UserImg="c:/blabla"},
            //    new User(){UserName="user4", Password="1234", Type=Types.Admin, LastLogin=DateTime.Now, UserImg="c:/blabla"}
            //};

            //var data = new DataTable("teste");
            //data.Columns.Add("col1", typeof(string));
            //data.Columns.Add("col2", typeof(string));

            //data.Rows.Add(1, "product 1");
            //data.Rows.Add(2, "product 2");
            //data.Rows.Add(3, "product 3");
            //data.Rows.Add(4, "product 4");
            //data.Rows.Add(5, "product 5");
            //data.Rows.Add(6, "product 6");
            //data.Rows.Add(7, "product 7");

            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment;filename=itfunda.xls");
            //Response.ContentType = "application/ms-excel";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Charset = "UTF-8";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
        public ActionResult ShowPDFReports(int Type, int Year, int Month)
        {
            DateTime? date = Services.Utilities.ValidateDate(Year, Month);
            ReportsViewModel vm = this.GetReportByType(Type, date);
            return View("ShowPDFReports", vm);
        }
        public ActionResult ExportReport(string type, string year, string month, string ex)
        {
            // handle with uncastable int

            int rType = 0;
            int rYear = 0;
            int rMonth = 0;

            Int32.TryParse(type, out rType);
            Int32.TryParse(year, out rYear);
            Int32.TryParse(month, out rMonth);

            SearchReportParamsViewModel searchParams = new SearchReportParamsViewModel()
            {
                Type = rType,
                Year = rYear,
                Month = rMonth,
                ExportTo = ex
            };

            if (searchParams.ExportTo == "xls")
            {
                DataTable table = this.GetDataTableToExcel(searchParams);
                return RedirectToAction("ExportToExcel", ExportToExcel(table));
            }
            else if (searchParams.ExportTo == "pdf")
            {
                return new ActionAsPdf("ShowPDFReports", new { Type = searchParams.Type, Year = searchParams.Year, Month = searchParams.Month }) { FileName = "TestPDF.pdf" };
            }
            else
            {
                return PartialView("ReportsError");
            }
        }
        public DataTable GetDataTableToExcel(SearchReportParamsViewModel searchParams)
        {
            int year = searchParams.Year;
            int month = searchParams.Month;
            DateTime? date = Services.Utilities.ValidateDate(year, month);
            ReportsViewModel vm = this.GetReportByType(searchParams.Type, date);
            
            var data = new DataTable("Report");

            if(vm.Type == ReportType.Charge)
            {
                data.Columns.Add("מס", typeof(string));
                data.Columns.Add("שם חניך", typeof(string));
                data.Columns.Add("ת\"ז חניך", typeof(string));
                data.Columns.Add("שם חונך", typeof(string));
                data.Columns.Add("ת\"ז חונך", typeof(string));
                data.Columns.Add("תאריך", typeof(string));
                data.Columns.Add("עלות לשעה", typeof(string));
                data.Columns.Add("סה\"כ שעות סיוע", typeof(string));
                data.Columns.Add("סה\"כ חיוב לתשלום", typeof(string));
                data.Columns.Add("מס תקציב", typeof(string));
                data.Columns.Add("שם גורם מאשר", typeof(string));
                data.Columns.Add("סטטוס", typeof(string));

                for (int i = 0; i < vm.Charges.Count(); i++)
                {
                    data.Rows.Add(i + 1, 
                                vm.Charges[i].StudentFullName, 
                                vm.Charges[i].StudentID,
                                vm.Charges[i].TeacherFullName,
                                vm.Charges[i].TeacherID,
                                vm.Charges[i].Date,
                                vm.Charges[i].CostPerHour,
                                vm.Charges[i].SumOfHours,
                                vm.Charges[i].SumOfBill,
                                vm.Charges[i].BudgetNumber,
                                vm.Charges[i].ManagerUserName,
                                vm.Charges[i].ManagerSignature);
                }
            }
            else if(vm.Type == ReportType.Refund)
            {
                data.Columns.Add("מס", typeof(string));
                data.Columns.Add("קוד טופס", typeof(string));
                data.Columns.Add("שם משתמש חונך", typeof(string));
                data.Columns.Add("שם חונך", typeof(string));
                data.Columns.Add("תאריך", typeof(string));
                data.Columns.Add("קוד קורס", typeof(string));
                data.Columns.Add("שם קורס", typeof(string));
                data.Columns.Add("שם חוג", typeof(string));
                data.Columns.Add("שם מרצה", typeof(string));
                data.Columns.Add("קבוצתי", typeof(string));
                data.Columns.Add("סהכ שעות לטופס", typeof(string));
                data.Columns.Add("שם מנהל", typeof(string));
                data.Columns.Add("מס תקציב", typeof(string));
                data.Columns.Add("מקור מימון", typeof(string));
                data.Columns.Add("אישור מנהל", typeof(string));
                data.Columns.Add("תאריך אישור", typeof(string));

                for (int i = 0; i < 100; i++)
                {
                    data.Rows.Add(i + 1,
                                vm.Refunds[i].ID,
                                vm.Refunds[i].TeacherUserName,
                                vm.Refunds[i].TeacherFullName,
                                vm.Refunds[i].Date,
                                vm.Refunds[i].CourseID,
                                vm.Refunds[i].CourseName,
                                vm.Refunds[i].DepartmentName,
                                vm.Refunds[i].LecturerName,
                                vm.Refunds[i].IsGrouped,
                                vm.Refunds[i].TotalHours,
                                vm.Refunds[i].ManagerFullName,
                                vm.Refunds[i].BudgetNumber,
                                vm.Refunds[i].SourceFund,
                                vm.Refunds[i].ManagerSignature,
                                vm.Refunds[i].SignatureDate);
                }
            }
            else if (vm.Type == ReportType.User)
            {

            }
            return data;
        }
        public ActionResult EditUsers()
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            ReportsViewModel reportsVM = new ReportsViewModel();
            List<UserReport> usersModel = bl.GetAllUsers();
            List<UsersReportViewModel> usesrVM = Services.ConverterService.ToUsersReportViewModel(usersModel);
            reportsVM.Users = usesrVM;
            return View("EditUsers", reportsVM);
        }
        public ActionResult EditUserByID(string username)
        {
            BussinesLayer bl = new BussinesLayer(new FinalDB());
            UserProfile userProfileModel = bl.GetUserProfileByUsername(username);
            UserProfileViewModel userProfileVM = new UserProfileViewModel()
            {
                Identity = userProfileModel.Identity,
                FirstName = userProfileModel.FirstName,
                LastName = userProfileModel.LastName,
                DepartmentID = userProfileModel.DepartmentID,
                Year = userProfileModel.Year,
                Gender = userProfileModel.Gender,
                Birth = userProfileModel.Birth.ToString("dd/MM/yyyy"),
                Phone = userProfileModel.Phone,
                UserName = userProfileModel.UserName,
                Password = userProfileModel.Password,
                Type = userProfileModel.Type,
                LastLogin = userProfileModel.LastLogin,
                UserImg = userProfileModel.UserImg,
                IsActive = userProfileModel.IsActive
                //UserProfileImages = (userProfileModel.Gender == "זכר") ? Services.Utilities.maleImgs : Services.Utilities.femaleImgs
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
            userProfileVM.Departments = departmentViewModelList;
            return View("EditUserByID", userProfileVM);
        }
        [HttpPost]
        public ActionResult UpdateUser(UserProfileViewModel userProfileVM)
        {
            string validationErrors = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
            Debug.WriteLine(validationErrors);

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
                }
                User userModel = new User()
                {
                    UserName = userProfileVM.UserName,
                    Password = userProfileVM.Password,
                    UserImg = userProfileVM.UserImg,
                    Type = userProfileVM.Type,
                    IsActive = userProfileVM.IsActive
                };
                bl.EditUser(userModel);
                Member memberModel = new Member()
                {
                    MemberUserName = userProfileVM.UserName,
                    Identity = userProfileVM.Identity,
                    DepartmentID = userProfileVM.DepartmentID,
                    Year = userProfileVM.Year,
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
            }
            else
            {
                fb = new FancyBox()
                {
                    Valid = false,
                    Message = "שדות לא תקינים"
                };
                TempData["FancyBox"] = fb;
            }
            return RedirectToAction("EditUsers");
        }

        public string Test()
        {
            User u = new User() { UserName="shimi", Password="1234"};

            string txt = "";
            foreach (var prop in u.GetType().GetProperties())
            {
                txt += prop.Name + " " + prop.GetValue(u, null) + " ";
            }
            return txt;
        }
    }
}