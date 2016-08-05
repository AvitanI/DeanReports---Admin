using DeanReports.DataAccessLayer;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    public class BussinesLayer
    {
        // instance variables

         private FinalDB dbContext;

        // constructor

        public BussinesLayer(FinalDB dbContext)
        {
            this.dbContext = dbContext;
        }

        // instance methods

        public void InsertDummyData()
        {
            List<User> users = new List<User>();

            for (int i = 0; i < 1000; i++)
            {
                Types type;
                if (i % 2 == 0)
                {
                    type = Types.Student;
                }
                else
                {
                    type = Types.Teacher;
                }

                users.Add(new User()
                {
                    UserName = "user" + i + "@gmail.com",
                    Password = "1234",
                    Type = type,
                    LastLogin = DateTime.Now,
                    UserImg = "/Content/images/avatars/boy1.png"
                });
            }

            users.Add(new User() { UserName = "student@gmail.com", Password = "1234", Type = Types.Student, LastLogin = DateTime.Now, UserImg = "/Content/images/avatars/boy1.png" });
            users.Add(new User() { UserName = "teacher@gmail.com", Password = "1234", Type = Types.Teacher, LastLogin = DateTime.Now, UserImg = "/Content/images/avatars/boy1.png" });
            users.Add(new User() { UserName = "admin@gmail.com", Password = "1234", Type = Types.Admin, LastLogin = DateTime.Now, UserImg = "/Content/images/avatars/boy1.png" });

            // create departments 

            List<Department> departments = new List<Department>()
            {
                new Department(){ID = 1, Name = "מערכות מידע ניהוליות"},
                new Department(){ID = 2, Name = "כלכלה וניהול"},
                new Department(){ID = 3, Name = "שירותי אנוש"},
                new Department(){ID = 4, Name = "מדעי התנהגות"},
                new Department(){ID = 5, Name = "פסיכולוגיה"},
                new Department(){ID = 6, Name = "חינוך מיוחד"},
                new Department(){ID = 7, Name = "קרימינולוגיה"},
                new Department(){ID = 8, Name = "סוציולוגיה ואנתרפולוגיה"},
                new Department(){ID = 9, Name = "מדעי המדינה"},
                new Department(){ID = 10, Name = "רב תחומי"},
                new Department(){ID = 11, Name = "מנהל מערכות בריאות"},
                new Department(){ID = 12, Name = "סיעוד"},
                new Department(){ID = 13, Name = "גורן"}
            };

            // create courses 

            List<Course> courses = new List<Course>();

            for (int i = 1; i < 51; i++)
            {
                courses.Add(new Course()
                {
                    ID = i,
                    Name = "course" + i
                });
            }

            List<CourseDepartments> CourseDepartments = new List<CourseDepartments>();

            for (int i = 1; i < 6; i++)
            {
                for (int j = 1; j < 51; j++)
                {
                    CourseDepartments.Add(new CourseDepartments()
                    {
                        DepartmentID = i,
                        CourseID = j
                    });
                }
            }

            // create members 

            List<Member> members = new List<Member>();
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int dept = rnd.Next(1, 6);
                int day = rnd.Next(1, 28);
                int month = rnd.Next(1, 13);
                int year = rnd.Next(1990, 2015);

                members.Add(new Member()
                {
                    MemberUserName = users.ElementAt(i).UserName,

                    DepartmentID = dept,
                    FirstName = "fname" + i,
                    LastName = "lname" + i,
                    Birth = new DateTime(year, month, day),
                    Phone = "000-000-0000",
                    Gender = "Male"
                });
            }

            members.Add(new Member(){ MemberUserName = "student@gmail.com", DepartmentID = 1, FirstName = "ניסוי", LastName = "ניסוי", Birth = new DateTime(2000, 1, 1), Phone = "000-000-0000", Gender="Male"});
            members.Add(new Member() { MemberUserName = "teacher@gmail.com", FirstName = "ניסוי", LastName = "ניסוי", Birth = new DateTime(2000, 1, 1), Phone = "000-000-0000", Gender = "Male" });
            members.Add(new Member() { MemberUserName = "admin@gmail.com", FirstName = "ניסוי", LastName = "ניסוי", Birth = new DateTime(2000, 1, 1), Phone = "000-000-0000", Gender = "Male" });

            users.Add(new User() { UserName = "admin1", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin, UserImg = "/Content/images/avatars/boy1.png" });
            users.Add(new User() { UserName = "admin2", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin, UserImg = "/Content/images/avatars/boy1.png" });
            users.Add(new User() { UserName = "admin3", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin, UserImg = "/Content/images/avatars/boy1.png" });
            users.Add(new User() { UserName = "admin4", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin, UserImg = "/Content/images/avatars/boy1.png" });

            // create managers

            List<Manager> managers = new List<Manager>()
            {
                new Manager(){UserName ="admin1", FirstName="ניזאר", LastName="ביטאר", Birth=DateTime.Now},
                new Manager(){UserName ="admin2", FirstName="מיכל", LastName="פישר", Birth=DateTime.Now},
                new Manager(){UserName ="admin3", FirstName="מעיין", LastName="אדלשטיין", Birth=DateTime.Now},
                new Manager(){UserName ="admin4", FirstName="מורן", LastName="בטרני", Birth=DateTime.Now}
            };

            // create programs

            List<Programs> programs = new List<Programs>()
            {
                new Programs(){ID = 1, Name="קרן אקדמיה"},
                new Programs(){ID = 2, Name="קרן קציר"},
                new Programs(){ID = 3, Name="חד הוריות"},
                new Programs(){ID = 4, Name="מנהל הסטודנטים"},
                new Programs(){ID = 5, Name="קרן אייסף"},
                new Programs(){ID = 6, Name="מרכז תמיכה ל\"ל"},
                new Programs(){ID = 7, Name="היחידה לקידום הסטודנט הערבי"},
                new Programs(){ID = 8, Name="טיפול דיקאנט"},
            };

            // create programs for each member

            List<ProgramsMembers> programsMembers = new List<ProgramsMembers>();

            foreach (var member in members)
            {
                int x = rnd.Next(1, 9);

                for (int i = x; i < programs.Count; i++)
                {
                    programsMembers.Add(new ProgramsMembers()
                    {
                        MemberUserName = member.MemberUserName,
                        ProgramsID = i
                    });
                }
            }

            // create refunds 

            List<Refund> refunds = new List<Refund>();
            //string[] months = {"january", "febuary", "march", "april", 
            //                   "may", "june", "july", "august", "september",
            //                  "october", "november", "december"};
            // get list of teachers
            var teachers = (from u in users
                            where u.Type == Types.Teacher
                            select u).ToList();


            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                int rand = r.Next(0, 12);
                int month = rnd.Next(1, 13);
                int year = rnd.Next(2013, 2017);
                int manager = rnd.Next(0, 3);
                int courseID = rnd.Next(1, 51);
                refunds.Add(new Refund()
                {
                    TeacherUserName = teachers.ElementAt(i).UserName,
                    Date = new DateTime(year, month, 1),
                    CourseID = courseID,
                    LecturerName = "test" + i,
                    ManagerUserName = managers.ElementAt(manager).UserName,
                    BudgetNumber = 0
                });
            }

            // create sessions 

            List<Session> sessions = new List<Session>();

            var students = (from u in users
                            where u.Type == Types.Student
                            select u).ToList();

            //Random rnd = new Random();
            bool sig = false;
            for (int i = 0; i < 400; i++)
            {
                if (i == 200)
                {
                    sig = true;
                }
                int refund = rnd.Next(0, 100);
                int day = rnd.Next(1, 28);
                int month = rnd.Next(1, 13);
                int year = rnd.Next(2013, 2017);

                int startH = rnd.Next(8, 15);
                int endH = rnd.Next(15, 21);
                TimeSpan startTime = new TimeSpan(startH, 0, 0);
                TimeSpan endTime = new TimeSpan(endH, 0, 0);
                TimeSpan duration = DateTime.Parse(endTime.ToString()).Subtract(DateTime.Parse(startTime.ToString()));
                sessions.Add(new Session()
                {
                    ID = i,
                    StudentUserName = students.ElementAt(i).UserName,
                    RefundID = refunds.ElementAt(refund).ID,
                    TeacherUserName = refunds.ElementAt(refund).TeacherUserName,
                    Date = new DateTime(year, month, day),
                    StartHour = startTime,
                    EndHour = endTime,
                    SumHoursPerSession = duration.Hours,
                    Details = "details",
                    StudentSignature = sig
                });
            }

            // create requests 

            List<Request> requests = new List<Request>();

            for (int j = 0; j < 100; j++)
            {
                int hours = rnd.Next(0, 31);
                int manager = rnd.Next(0, 3);
                requests.Add(new Request()
                {
                    ID = j,
                    StudentUserName = students.ElementAt(j).UserName,
                    Type = "test" + j,
                    Cause = "test" + j,
                    FormType = "כללי",
                    Date = DateTime.Now,
                    ManagerUserName = managers.ElementAt(manager).UserName,
                    ApprovalHours = hours,
                    BudgetNumber = 0,
                    Notes = "notes",
                    ManagerSignature = true,
                    SignatureDate = DateTime.Now
                });
            }

            List<CourseRequest> courseRequests = new List<CourseRequest>();

            for (int i = 0; i < 100; i++)
            {
                int course = rnd.Next(1, 51);
                int counter = 0;
                for (int j = course; j < courses.Count; j++)
                {
                    counter++;
                    if (counter == 4)
                    {
                        break;
                    }
                    courseRequests.Add(new CourseRequest()
                    {
                        RequestID = requests.ElementAt(i).ID,
                        StudentUserName = requests.ElementAt(i).StudentUserName,
                        CourseID = j,
                        LecturerName = "LecturerName" + i
                    });
                }
            }

            dbContext.Users.AddRange(users);
            dbContext.Departments.AddRange(departments);
            dbContext.Courses.AddRange(courses);
            dbContext.CourseDepartments.AddRange(CourseDepartments);
            dbContext.Members.AddRange(members);
            dbContext.Managers.AddRange(managers);
            dbContext.Programs.AddRange(programs);
            dbContext.ProgramsMembers.AddRange(programsMembers);
            dbContext.Refunds.AddRange(refunds);
            dbContext.Sessions.AddRange(sessions);
            dbContext.Requests.AddRange(requests);
            dbContext.CourseRequest.AddRange(courseRequests);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }

        // user profile
        public UserProfile GetUserProfileByUsername(string username)
        {
            try
            {
                Object[] parameters = { new SqlParameter("UserName", username) };
                UserProfile userProfile = dbContext.Database.SqlQuery<UserProfile>(@"GetUserProfileByUsername @UserName", parameters).Single();
                return userProfile;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetUserProfileByUsername function: " + e);
                return new UserProfile();
            }
        }

        // user section
        public bool IsUserExist(string username)
        {
            try
            {
                User user = dbContext.Users.SingleOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    return true;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return false;
        }
        public User GetUserValidity(User user)
        {
            try
            {
                User result = dbContext.Users.SingleOrDefault(u => u.UserName == user.UserName && 
                                                                    u.Password == user.Password);

                if (result != null)
                {
                    return result;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return new User() { Type = Types.NonUser };
        }
        public User GetUserByUsername(string username)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("UserName", username)
                };
                User u = dbContext.Database.SqlQuery<User>(@"GetUserByUsername @UserName", parameters).ToList()[0];
                return u;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetUserByUsername function: " + e);
                return new User();
            }
        }
        public bool AddUser(User u)
        {
            Debug.WriteLine(u.UserName + " || " + u.Password + " || " + u.LastLogin + " || " + u.Type);
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("UserName", u.UserName),
                    new SqlParameter("Password", u.Password),
                    new SqlParameter("Type", u.Type),
                    new SqlParameter("LastLogin", u.LastLogin),
                    new SqlParameter("UserImg", u.UserImg)
                };

                dbContext.Database.ExecuteSqlCommand("Create_User @UserName, @Password, @Type, @LastLogin, @UserImg", parameters);
                int num = dbContext.SaveChanges();
                Debug.WriteLine("Numbers of rows " + num);
                return true;

            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add User function: " + e);
                return false;
            }
        }
        public bool EditUser(User u)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("UserName", u.UserName),
                    new SqlParameter("Password", u.Password),
                    new SqlParameter("UserImg", u.UserImg)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_User @UserName, @Password, @UserImg", parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit User Details function: " + e);
                return false;
            }
        }
        public bool RemoveUser(User u)
        {
            try
            {
                var userName = new SqlParameter("UserName", u.UserName);
                dbContext.Database.ExecuteSqlCommand("Delete_User @UserName", userName);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete User Details function: " + e);
                return false;
            }
        }
        public bool UpdateLastLogin(User u)
        {
            try
            {
                object[] parameters =
                {
                    new SqlParameter("UserName", u.UserName),
                    new SqlParameter("LastLogin", u.LastLogin)
                };
                dbContext.Database.ExecuteSqlCommand("UpdateLastLogin @UserName, @LastLogin", parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Update Last Login Details function: " + e);
                return false;
            }
        }

        // member section
        public Member IsMemberExist(string username)
        {
            try
            {
                Member member = dbContext.Members.SingleOrDefault(m => m.MemberUserName == username);
                if (member != null)
                {
                    return member;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public bool AddMember(Member m)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberUserName", m.MemberUserName),
                    new SqlParameter("Identity", m.Identity),
                    new SqlParameter("DepartmentID", m.DepartmentID),
                    new SqlParameter("Year", m.Year),
                    new SqlParameter("FirstName", m.FirstName),
                    new SqlParameter("LastName", m.LastName),
                    new SqlParameter("Birth", m.Birth),
                    new SqlParameter("Phone", m.Phone),
                    new SqlParameter("Gender", m.Gender)
                };
                dbContext.Database.ExecuteSqlCommand(@"Create_Member @MemberUserName, @Identity, @DepartmentID, @Year, 
                                                                    @FirstName, @LastName, @Birth, @Phone, @Gender",
                                                                    parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Member function: " + e);
                return false;
            }
        }
        public bool EditMember(Member m)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberUserName", m.MemberUserName),
                    new SqlParameter("DepartmentID", m.DepartmentID),
                    new SqlParameter("Year", m.Year),
                    new SqlParameter("FirstName", m.FirstName),
                    new SqlParameter("LastName", m.LastName),
                    new SqlParameter("Birth", m.Birth),
                    new SqlParameter("Phone", m.Phone),
                    new SqlParameter("Gender", m.Gender)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_Member @MemberUserName, @DepartmentID, @Year, 
                                                                    @FirstName, @LastName, @Birth, @Phone, @Gender",
                                                                    parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Member Details function: " + e);
                return false;
            }
        }
        public bool RemoveMember(Member m)
        {
            try
            {
                var memberUserName = new SqlParameter("MemberUserName", m.MemberUserName);
                dbContext.Database.ExecuteSqlCommand(@"Delete_Member @MemberUserName", memberUserName);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Member Details function: " + e);
                return false;
            }
        }
        public List<Member> GetAllMembers()
        {
            try
            {
                List<Member> members = dbContext.Database.SqlQuery<Member>(@"GetAllMembers").ToList();
                return members;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetAllMembers function: " + e);
                return new List<Member>();
            }
        }
        public List<Member>GetAllMembersByType(Types type)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberType", type)
                };
                List<Member> members = dbContext.Database.SqlQuery<Member>(@"GetAllMembersByType @MemberType", parameters).ToList();
                return members;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetAllMembersByType function: " + e);
                return new List<Member>();
            }
        }
        public Member GetMemberByUsername(string username)
        {
            try
            {
                Object[] parameters = { new SqlParameter("MemberUserName", username) };
                Member member = dbContext.Database.SqlQuery<Member>(@"GetMemberByUsername @MemberUserName", parameters).FirstOrDefault();
                return member;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetMemberByUsername function: " + e);
                return new Member();
            }
        }

        // programs
        public List<Programs> GetAllPrograms()
        {
            try
            {
                List<Programs> programs = dbContext.Database.SqlQuery<Programs>(@"GetAllPrograms").ToList();
                return programs;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetAllPrograms function: " + e);
                return new List<Programs>();
            }
        }
        
        // departments
        public List<Department> GetAllDepartments()
        {
            try
            {
                List<Department> departments = dbContext.Database.SqlQuery<Department>(@"GetAllDepartments").ToList();
                return departments;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetAllDepartments function: " + e);
                return new List<Department>();
            }
        }

        // programsMembers section
        public ProgramsMembers IsProgramsMembersExist(int programsID, string username)
        {
            try
            {
                ProgramsMembers programsMembers = dbContext.ProgramsMembers.SingleOrDefault(p => p.ProgramsID == programsID && p.MemberUserName == username);
                // in case programsMembers is exist
                if (programsMembers != null)
                {
                    return programsMembers;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public bool AddProgramsMembers(ProgramsMembers pm)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberUserName", pm.MemberUserName),
                    new SqlParameter("ProgramsID", pm.ProgramsID)
                };
                dbContext.Database.ExecuteSqlCommand("Create_ProgramsMembers @MemberUserName, @ProgramsID",
                                                                              parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add ProgramsMembers function: " + e);
                return false;
            }
        }
        public bool EditProgramsMembers(int programsID, ProgramsMembers newPM)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberUserName", newPM.MemberUserName),
                    new SqlParameter("NewProgramsID", newPM.ProgramsID),
                    new SqlParameter("OldProgramsID", programsID)
                };

                dbContext.Database.ExecuteSqlCommand("Update_ProgramsMembers @MemberUserName, @OldProgramsID, @NewProgramsID",
                                                                              parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit ProgramsMembers Details function: " + e);
                return false;
            }
        }
        public bool RemoveProgramsMembers(ProgramsMembers PM)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("MemberUserName", PM.MemberUserName),
                    new SqlParameter("ProgramsID", PM.ProgramsID)
                };
                dbContext.Database.ExecuteSqlCommand("Delete_ProgramsMembers @MemberUserName, @ProgramsID",
                                                                              parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete ProgramsMembers Details function: " + e);
                return false;
            }
        }

        // request section
        public Request IsRequestExist(int requestID)
        {
            try
            {
                Request request = dbContext.Requests.SingleOrDefault(r => r.ID == requestID);
                if (request != null)
                {
                    return request;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public int AddRequest(Request r)
        {
            try
            {
                object[] parameters =
                {
                    new SqlParameter("StudentUserName", r.StudentUserName),
                    new SqlParameter("Type", r.Type),
                    new SqlParameter("Cause", r.Cause),
                    new SqlParameter("FormType", r.FormType),
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("ManagerUserName", r.ManagerUserName ?? SqlString.Null),
                    new SqlParameter("ApprovalHours", r.ApprovalHours ?? SqlInt32.Null),
                    new SqlParameter("BudgetNumber", r.BudgetNumber ?? SqlInt32.Null),
                    new SqlParameter("Notes", r.Notes ?? SqlString.Null),
                    new SqlParameter("ManagerSignature", r.ManagerSignature ?? SqlBoolean.Null),
                    new SqlParameter("SignatureDate", r.SignatureDate ?? SqlDateTime.Null),
                };

                decimal x = dbContext.Database.SqlQuery<decimal>(@"Create_Request @StudentUserName, @Type, @Cause, @FormType, @Date, @ManagerUserName, 
                                                                       @ApprovalHours, @BudgetNumber, @Notes, @ManagerSignature, @SignatureDate",
                                                                        parameters).First();
                dbContext.SaveChanges();
                return (int)x;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Request function: " + e);
                return -1;
            }
        }
        public bool EditRequest(Request r)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", r.ID),
                    new SqlParameter("StudentUserName", r.StudentUserName),
                    new SqlParameter("Type", r.Type),
                    new SqlParameter("Cause", r.Cause),
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("ManagerUserName", r.ManagerUserName ?? SqlString.Null),
                    new SqlParameter("ApprovalHours", r.ApprovalHours ?? SqlInt32.Null),
                    new SqlParameter("BudgetNumber", r.BudgetNumber ?? SqlInt32.Null),
                    new SqlParameter("Notes", r.Notes ?? SqlString.Null),
                    new SqlParameter("ManagerSignature", r.ManagerSignature ?? SqlBoolean.Null),
                    new SqlParameter("SignatureDate", r.SignatureDate ?? SqlDateTime.Null)
                };

                dbContext.Database.ExecuteSqlCommand(@"Update_Request @ID, @StudentUserName, @Type, @Cause, @Date, @ManagerUserName, 
                                                                       @ApprovalHours, @BudgetNumber, @Notes, @ManagerSignature, @SignatureDate",
                                                                        parameters);
                dbContext.SaveChanges();
                //Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Request Details function: " + e);
                return false;
            }
        }
        public bool RemoveRequest(int requestID)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", requestID)
                };
                dbContext.Database.ExecuteSqlCommand("Delete_Request @ID", parameters);
                dbContext.SaveChanges();
                //Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Request Details function: " + e);
                return false;
            }
        }
        public List<Request> GetRequestsByMemberID(string StudentUserName)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", StudentUserName)
                };
                List<Request> requests = dbContext.Database.SqlQuery<Request>("GetRequestsByMemberID @StudentUserName", parameters).ToList();
                return requests;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Get Request By Member Request Details function: " + e);
                return new List<Request>();
            }
        }
        public Request GetRequestByID(int requestID)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", requestID)
                };
                Request request = dbContext.Database.SqlQuery<Request>("GetRequestByID @ID", parameters).Single();
                return request;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetRequestByID function: " + e);
                return new Request();
            }
        }

        // course request section
        public CourseRequest IsCourseRequest(int requestID, string studentUserName, int courseID)
        {
            try
            {
                CourseRequest courseRequest = dbContext.CourseRequest.SingleOrDefault(c => c.RequestID == requestID &&
                                                                                           c.StudentUserName == studentUserName &&
                                                                                           c.CourseID == courseID);
                if (courseRequest != null)
                {
                    return courseRequest;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public bool AddCourseRequest(CourseRequest cr)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("RequestID", cr.RequestID),
                    new SqlParameter("StudentUserName", cr.StudentUserName),
                    new SqlParameter("CourseID", cr.CourseID),
                    new SqlParameter("LecturerName", cr.LecturerName)
                };
                dbContext.Database.ExecuteSqlCommand("Create_CourseRequest @RequestID, @StudentUserName, @CourseID, @LecturerName",
                                                                            parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Course Request Details function: " + e);
                return false;
            }
        }
        public bool EditCourseRequest(int courseID, CourseRequest newCR)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("RequestID", newCR.RequestID),
                    new SqlParameter("StudentUserName", newCR.StudentUserName),
                    new SqlParameter("NewCourseID", newCR.CourseID),
                    new SqlParameter("OldCourseID", courseID),
                    new SqlParameter("LecturerName", newCR.LecturerName)
                };
                dbContext.Database.ExecuteSqlCommand("Update_CourseRequest @RequestID, @StudentUserName, @NewCourseID, @OldCourseID, @LecturerName",
                                                                            parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Course Request Details function: " + e);
                return false;
            }
        }
        public bool RemoveCourseRequest(CourseRequest cr)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("RequestID", cr.RequestID),
                    new SqlParameter("StudentUserName", cr.StudentUserName),
                    new SqlParameter("CourseID", cr.CourseID)
                };
                dbContext.Database.ExecuteSqlCommand("Delete_CourseRequest @RequestID, @StudentUserName, @CourseID",
                                                                            parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Course Request Details function: " + e);
                return false;
            }
        }
        public List<CourseRequest> GetCourseRequestsByRequestID(int requestID)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("RequestID", requestID)
                };
                List<CourseRequest> courseRequest = dbContext.Database.SqlQuery<CourseRequest>("GetCourseRequestsByRequestID @RequestID", parameters).ToList();
                return courseRequest;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Get Request By Member Request Details function: " + e);
                return new List<CourseRequest>();
            }
        }

        // refund section
        public Refund IsRefundExist(int refundID)
        {
            try
            {
                Refund refund = dbContext.Refunds.SingleOrDefault(r => r.ID == refundID);
                if (refund != null)
                {
                    return refund;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public int AddRefund(Refund r)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("TeacherUserName", r.TeacherUserName),
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("CourseID", r.CourseID),
                    new SqlParameter("LecturerName", r.LecturerName),
                    new SqlParameter("IsGrouped", r.IsGrouped),
                    new SqlParameter("ManagerUserName", r.ManagerUserName ?? SqlString.Null),
                    new SqlParameter("BudgetNumber", r.BudgetNumber ?? SqlInt32.Null)
                };

                decimal x = dbContext.Database.SqlQuery<decimal>("Create_Refund @TeacherUserName, @Date, @CourseID, @LecturerName, @IsGrouped, @ManagerUserName, @BudgetNumber",
                                                                          parameters).First();
                dbContext.SaveChanges();
                return (int)x;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Refund function: " + e);
                return -1;
            }
        }
        public bool EditRefund(Refund r)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", r.ID),
                    new SqlParameter("TeacherUserName", r.TeacherUserName),
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("CourseID", r.CourseID),
                    new SqlParameter("LecturerName", r.LecturerName),
                    new SqlParameter("IsGrouped", r.IsGrouped),
                    new SqlParameter("ManagerUserName", r.ManagerUserName),
                    new SqlParameter("BudgetNumber", r.BudgetNumber)
                };
                dbContext.Database.ExecuteSqlCommand("Update_Refund @ID, @Date, @CourseName, @LecturerName, @IsGrouped, @ManagerUserName, @BudgetNumber",
                                                                          parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Refund Details function: " + e);
                return false;
            }
        }
        public bool RemoveRefund(Refund r)
        {
            try
            {
                var ID = new SqlParameter("ID", r.ID);
                dbContext.Database.ExecuteSqlCommand("Delete_Refund @ID", ID);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Refund Details function: " + e);
                return false;
            }
        }
        public List<Refund> GetRefundsByMemberID(string teacherUserName)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("TeacherUserName", teacherUserName)
                };
                List<Refund> refunds = dbContext.Database.SqlQuery<Refund>("GetRefundsByMemberID @TeacherUserName", parameters).ToList();
                dbContext.SaveChanges();
                return refunds;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetRefundsByMemberID function: " + e);
                return new List<Refund>();
            }
        }
        public Refund GetRefundByID(int refundID)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", refundID)
                };
                Refund refund = dbContext.Database.SqlQuery<Refund>("GetRefundByID @ID", parameters).Single();
                return refund;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetRefundByID function: " + e);
                return new Refund();
            }
        }

        // session section
        public Session IsSessionExist(int sessionID)
        {
            try
            {
                Session session = dbContext.Sessions.SingleOrDefault(s => s.ID == sessionID);
                if (session != null)
                {
                    return session;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public bool AddSession(Session s)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", s.StudentUserName),
                    new SqlParameter("RefundID", s.RefundID),
                    new SqlParameter("TeacherUserName", s.TeacherUserName ?? SqlString.Null),
                    new SqlParameter("Date", s.Date),
                    new SqlParameter("StartHour", s.StartHour),
                    new SqlParameter("EndHour", s.EndHour),
                    new SqlParameter("SumOfHoursPerSession", s.SumHoursPerSession),
                    new SqlParameter("Details", s.Details ?? SqlString.Null),
                    new SqlParameter("StudentSignature", false)
                };
                dbContext.Database.ExecuteSqlCommand(@"Create_Session @StudentUserName, @RefundID, @TeacherUserName, @Date,
                                                                          @StartHour, @EndHour, @SumOfHoursPerSession, @Details, @StudentSignature",
                                                                     parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Session function: " + e);
                return false;
            }
        }
        public bool EditSession(Session s)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", s.ID),
                    new SqlParameter("RefundID", s.RefundID),
                    new SqlParameter("TeacherUserName", s.TeacherUserName),
                    new SqlParameter("Date", s.Date),
                    new SqlParameter("StartHour", s.StartHour),
                    new SqlParameter("EndHour", s.EndHour),
                    new SqlParameter("SumOfHoursPerSession", s.SumHoursPerSession),
                    new SqlParameter("Details", s.Details),
                    new SqlParameter("StudentSignature", s.StudentSignature)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_Session @ID, @RefundID, @TeacherUserName, @Date
                                                                          @StartHour, @EndHour, @SumOfHoursPerSession, @Details, @StudentSignature",
                                                                     parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Session Details function: " + e);
                return false;
            }
        }
        public bool RemoveSession(int sessionID)
        {
            try
            {
                var ID = new SqlParameter("ID", sessionID);
                dbContext.Database.ExecuteSqlCommand(@"Delete_Session @ID", ID);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Sessios Details function: " + e);
                return false;
            }
        }
        public List<Session> GetSessionsByRefundID(int refundID)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("RefundID", refundID)
                };
                List<Session> sessions = dbContext.Database.SqlQuery<Session>("GetSessionsByRefundID @RefundID", parameters).ToList();
                return sessions;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetSessionsByRefundID function: " + e);
                return new List<Session>();
            }
        }
        public List<Session> GetSessionsByMemberID(string studentUsername)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", studentUsername)
                };
                List<Session> sessions = dbContext.Database.SqlQuery<Session>("GetSessionsByMemberID @StudentUserName", parameters).ToList();
                return sessions;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetSessionsByMemberID function: " + e);
                return new List<Session>();
            }
        }
        public bool ConfirmSessionByID(int sessionID)
        {
            try
            {
                var ID = new SqlParameter("ID", sessionID);
                dbContext.Database.ExecuteSqlCommand(@"ConfirmSessionByID @ID", ID);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with ConfirmSessionByID function: " + e);
                return false;
            }
        }
        public List<Session> GetDuplicateSessions(Session s)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("TeacherUserName", s.TeacherUserName),
                    new SqlParameter("Date", s.Date),
                    new SqlParameter("StartHour", s.StartHour),
                    new SqlParameter("EndHour", s.EndHour)
                };
                List<Session> sessions = dbContext.Database.SqlQuery<Session>("GetDuplicateSessions @TeacherUserName, @Date, @StartHour, @EndHour", parameters).ToList();
                return sessions;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetDuplicateSessions function: " + e);
                return new List<Session>();
            }
        }
        public List<Session> GetDuplicateSessionsByStudentUsername(Session s)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", s.StudentUserName),
                    new SqlParameter("Date", s.Date),
                    new SqlParameter("StartHour", s.StartHour),
                    new SqlParameter("EndHour", s.EndHour)
                };
                List<Session> sessions = dbContext.Database.SqlQuery<Session>("GetDuplicateSessionsByStudentUsername @StudentUserName, @Date, @StartHour, @EndHour", parameters).ToList();
                return sessions;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetDuplicateSessionsByStudentUsername function: " + e);
                return new List<Session>();
            }
        }
        
        // charge section
        public Charge IsChargeExist(int chargeID)
        {
            try
            {
                Charge charge = dbContext.Charges.SingleOrDefault(c => c.ID == chargeID);
                if (charge != null)
                {
                    return charge;
                }
            }
            catch
            {
                Debug.WriteLine("There is more than one result!");
            }
            return null;
        }
        public bool AddCharge(Charge c)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", c.StudentUserName),
                    new SqlParameter("TeacherUserName", c.TeacherUserName),
                    new SqlParameter("Period", c.Period),
                    new SqlParameter("CostPerHour", c.CostPerHour),
                    new SqlParameter("SumOfHours", c.SumOfHours),
                    new SqlParameter("SumOfBill", c.SumOfBill),
                    new SqlParameter("Notes", c.Notes),
                    new SqlParameter("Date", c.Date),
                    new SqlParameter("ManagerUserName", c.ManagerUserName),
                    new SqlParameter("BudgetNumber", c.BudgetNumber),
                    new SqlParameter("FundSource", c.FundSource),
                    new SqlParameter("ManagerSignature", c.ManagerSignature),
                    new SqlParameter("SignatureDate", c.SignatureDate)
                };
                dbContext.Database.ExecuteSqlCommand(@"Create_Charge @StudentUserName, @TeacherUserName, @Period, @CostPerHour, @SumOfHours, @SumOfBill,
                                                                    @Notes, @Date, @ManagerUserName, @BudgetNumber, @FundSource, @ManagerSignature, @SignatureDate",
                                                                     parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Charge function: " + e);
            }
            return false;
        }
        public bool EditCharge(Charge c)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("TeacherUserName", c.TeacherUserName),
                    new SqlParameter("Period", c.Period),
                    new SqlParameter("CostPerHour", c.CostPerHour),
                    new SqlParameter("SumOfHours", c.SumOfHours),
                    new SqlParameter("SumOfBill", c.SumOfBill),
                    new SqlParameter("Notes", c.Notes),
                    new SqlParameter("Date", c.Date),
                    new SqlParameter("ManagerUserName", c.ManagerUserName),
                    new SqlParameter("BudgetNumber", c.BudgetNumber),
                    new SqlParameter("FundSource", c.FundSource),
                    new SqlParameter("ManagerSignature", c.ManagerSignature),
                    new SqlParameter("SignatureDate", c.SignatureDate)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_Charge @TeacherUserName, @Period, @CostPerHour, @SumOfHours, @SumOfBill,
                                                                    @Notes, @Date, @ManagerUserName, @BudgetNumber, @FundSource, @ManagerSignature, @SignatureDate",
                                                                     parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Charge Details function: " + e);
                return false;
            }
        }
        public bool RemoveCharge(Charge c)
        {
            try
            {
                var ID = new SqlParameter("ID", c.ID);
                dbContext.Database.ExecuteSqlCommand("Delete_Charge @ID", ID);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Charge Details function: " + e);
                return false;
            }
        }

        // course section
        public List<Course> GetCoursesByDepartmentID(int departmentID)
        {
            try
            {
                var ID = new SqlParameter("DepartmentID", departmentID);
                List<Course> courses = dbContext.Database.SqlQuery<Course>(@"GetCoursesByDepartmentID @DepartmentID", ID).ToList();
                return courses;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetCoursesByDepartmentID function: " + e);
                return new List<Course>();
            }
        }
    
        // teacher bussiness

        public List<Refund> GetTeacherRefunds(string teacherUserName)
        {
            List<Refund> refunds = this.GetRefundsByMemberID(teacherUserName);

            foreach (Refund refund in refunds)
            {
                refund.Sessions = this.GetSessionsByRefundID(refund.ID);
            }
            return refunds;
        }

        // admin bussiness

        public List<Request> GetNonConfirmedRequests()
        {
            try
            {
                List<Request> requests = dbContext.Database.SqlQuery<Request>("GetNonConfirmedRequests").ToList();
                return requests;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetNonConfirmedRequests function: " + e);
                return new List<Request>();
            }
        }
        public bool AddMessage(Messages message)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("Type", message.Type),
                    new SqlParameter("From", message.From),
                    new SqlParameter("ToUser", message.ToUser),
                    new SqlParameter("Subject", message.Subject),
                    new SqlParameter("Content", message.Content),
                    new SqlParameter("Date", message.Date),
                    new SqlParameter("IsSeen", message.IsSeen)
                };

                dbContext.Database.ExecuteSqlCommand("Create_Message @Type, @From, @ToUser, @Subject, @Content, @Date, @IsSeen", parameters);
                return true;

            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with AddMessage function: " + e);
                return false;
            }
        }
        public List<Messages> GetMessagesByUser(string username)
        {
            try
            {
                var Username = new SqlParameter("Username", username);
                List<Messages> messages = dbContext.Database.SqlQuery<Messages>(@"GetMessagesByUser @Username", Username).ToList();
                return messages;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetMessagesByUser function: " + e);
                return new List<Messages>();
            }
        }
        public List<Member> GetMemberByAjax(string query)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("word", query)
                };
                List<Member> member = dbContext.Database.SqlQuery<Member>("GetMemberByAjax @word", parameters).ToList();
                return member;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with GetMemberByAjax function: " + e);
                return new List<Member>();
            }
        }
    
    }
}