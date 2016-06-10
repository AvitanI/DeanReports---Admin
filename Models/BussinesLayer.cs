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
                });
            }

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
                    Phone = "000-000-0000"
                });
            }

            users.Add(new User() { UserName = "admin1", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin });
            users.Add(new User() { UserName = "admin2", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin });
            users.Add(new User() { UserName = "admin3", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin });
            users.Add(new User() { UserName = "admin4", Password = "1234", LastLogin = DateTime.Now, Type = Types.Admin });

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
        public bool AddUser(User u)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("UserName", u.UserName),
                    new SqlParameter("Password", u.Password),
                    new SqlParameter("Type", u.Type),
                    new SqlParameter("LastLogin", u.LastLogin),
                };

                dbContext.Database.ExecuteSqlCommand("Create_User @UserName, @Password, @Type, @LastLogin", parameters);
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
                    new SqlParameter("Password", u.Password)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_User @UserName, @Password", parameters);
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
                Console.WriteLine("success");
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
                    new SqlParameter("Phone", m.Phone)
                };
                dbContext.Database.ExecuteSqlCommand(@"Create_Member @MemberUserName, @Identity, @DepartmentID, @Year, 
                                                                    @FirstName, @LastName, @Birth, @Phone",
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
                    new SqlParameter("FirstName", m.FirstName),
                    new SqlParameter("LastName", m.LastName),
                    new SqlParameter("Birth", m.Birth),
                    new SqlParameter("Phone", m.Phone)
                };
                dbContext.Database.ExecuteSqlCommand(@"Update_Member @MemberUserName, @DepartmentID, 
                                                                    @FirstName, @LastName, @Birth, @Phone",
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
        public Member GetMemberByUsername(string username)
        {
            try
            {
                Object[] parameters = { new SqlParameter("MemberUserName", username) };
                Member member = dbContext.Database.SqlQuery<Member>(@"GetMemberByUsername @MemberUserName", parameters).ToList()[0];
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
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("ManagerUserName", r.ManagerUserName ?? SqlString.Null),
                    new SqlParameter("ApprovalHours", r.ApprovalHours ?? SqlInt32.Null),
                    new SqlParameter("BudgetNumber", r.BudgetNumber ?? SqlInt32.Null),
                    new SqlParameter("Notes", r.Notes ?? SqlString.Null),
                    new SqlParameter("ManagerSignature", r.ManagerSignature ?? SqlBoolean.Null),
                    new SqlParameter("SignatureDate", r.SignatureDate ?? SqlDateTime.Null),
                };

                decimal x = dbContext.Database.SqlQuery<decimal>(@"Create_Request @StudentUserName, @Type, @Cause, @Date, @ManagerUserName, 
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
                    new SqlParameter("Date", r.Date),
                    new SqlParameter("ManagerUserName", r.ManagerUserName),
                    new SqlParameter("ApprovalHours", r.ApprovalHours),
                    new SqlParameter("BudgetNumber", r.BudgetNumber),
                    new SqlParameter("Notes", r.Notes),
                    new SqlParameter("ManagerSignature", r.ManagerSignature),
                    new SqlParameter("SignatureDate", r.SignatureDate)
                };

                dbContext.Database.ExecuteSqlCommand(@"Update_Request @ID, @StudentUserName, @Type, @Cause, @Date, @ManagerUserName, 
                                                                       @ApprovalHours, @BudgetNumber, @Notes, @ManagerSignature, @SignatureDate",
                                                                        parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Edit Request Details function: " + e);
                return false;
            }
        }
        public bool RemoveRequest(Request r)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("ID", r.ID),
                    new SqlParameter("StudentUserName", r.StudentUserName)
                };
                dbContext.Database.ExecuteSqlCommand("Delete_Request @ID, @StudentUserName", parameters);
                dbContext.SaveChanges();
                Console.WriteLine("success");
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Request Details function: " + e);
                return false;
            }
        }
        public List<Request> GetRequestsByMemberID(string StudentUserName, DateTime date)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("StudentUserName", StudentUserName),
                    new SqlParameter("Date", date)
                };
                List<Request> requests = dbContext.Database.SqlQuery<Request>("GetRequestsByMemberID @StudentUserName, @Date", parameters).ToList();
                dbContext.SaveChanges();
                return requests;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Get Request By Member Request Details function: " + e);
                return new List<Request>();
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
        public List<CourseRequest> GetCourseRequests(int requestID, string studentUserName)
        {
            List<CourseRequest> courseRequests = dbContext.Database.SqlQuery<CourseRequest>(@"SELECT * 
                                                                            FROM [dbo].[CourseRequest]
                                                                            WHERE RequestID = '" + requestID +
                                                                         "' StudentUserName = '" + studentUserName + "';").ToList();
            return courseRequests;
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
        public bool AddRefund(Refund r)
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
                    new SqlParameter("ManagerUserName", r.ManagerUserName ?? SqlString.Null),
                    new SqlParameter("BudgetNumber", r.BudgetNumber ?? SqlInt32.Null)
                };

                dbContext.Database.ExecuteSqlCommand("Create_Refund @ID, @TeacherUserName, @Date, @CourseName, @LecturerName, @ManagerUserName, @BudgetNumber",
                                                                          parameters);
                dbContext.SaveChanges();
                return true;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Add Refund function: " + e);
                return false;
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
                    new SqlParameter("ManagerUserName", r.ManagerUserName),
                    new SqlParameter("BudgetNumber", r.BudgetNumber)
                };
                dbContext.Database.ExecuteSqlCommand("Update_Refund @ID, @Date, @CourseName, @LecturerName, @ManagerUserName, @BudgetNumber",
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
        public List<Refund> GetRefundsByMemberID(string teacherUserName, DateTime date)
        {
            try
            {
                Object[] parameters =
                {
                    new SqlParameter("TeacherUserName", teacherUserName),
                    new SqlParameter("Date", date)
                };
                List<Refund> refunds = dbContext.Database.SqlQuery<Refund>("GetRefundsByMemberID @TeacherUserName, @Date", parameters).ToList();
                dbContext.SaveChanges();
                return refunds;
            }
            catch (SqlException e)
            {
                Debug.WriteLine("Problem with Delete Get Refund By Member Request Details function: " + e);
                return new List<Refund>();
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
                    new SqlParameter("ID", s.ID),
                    new SqlParameter("StudentUserName", s.StudentUserName),
                    new SqlParameter("RefundID", s.RefundID),
                    new SqlParameter("TeacherUserName", s.TeacherUserName ?? SqlString.Null),
                    new SqlParameter("Date", s.Date),
                    new SqlParameter("StartHour", s.StartHour),
                    new SqlParameter("EndHour", s.EndHour),
                    new SqlParameter("SumOfHoursPerSession", s.SumHoursPerSession),
                    new SqlParameter("Details", s.Details ?? SqlString.Null),
                    new SqlParameter("StudentSignature", s.StudentSignature ?? SqlBoolean.Null)
                };
                dbContext.Database.ExecuteSqlCommand(@"Create_Session @ID, @StudentUserName, @RefundID, @TeacherUserName, @Date,
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
        public bool RemoveSession(Session s)
        {
            try
            {
                var ID = new SqlParameter("ID", s.ID);
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
        public List<Session> GetSessions(string StudentUserName, DateTime date)
        {
            List<Session> sessions = dbContext.Database.SqlQuery<Session>(@"SELECT * 
                                                                        FROM [dbo].[Session]
                                                                        WHERE StudentUserName = '" + StudentUserName +
                                                                        "' AND year(Date) = '" + date.Year +
                                                                        "' AND month(Date) = '" + date.Month + "';").ToList();
            return sessions;
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

        // student bussiness
        public RequestListViewModel GetStudentRequests(string StudentUserName, DateTime date)
        {
            // create list of request instance
            RequestListViewModel listRequestsViewModel = new RequestListViewModel();
            // initialize list of requsets
            List<RequestViewModel> list = new List<RequestViewModel>();

            try
            {
                List<Request> requests = this.GetRequestsByMemberID(StudentUserName, date);
                // for each request get the matching courses
                foreach (var item in requests)
                {
                    try
                    {
                        List<CourseRequestViewModel> courseRequestsViewModel = dbContext.Database.SqlQuery<CourseRequestViewModel>(@"SELECT c.ID as CourseID,
                                                                                                                                            c.Name as Name,
                                                                                                                                            cr.LecturerName as LecturerName
                                                                                                                                     FROM [dbo].[CourseRequest] cr
                                                                                                                                     INNER JOIN [dbo].[Course] c
                                                                                                                                     ON cr.CourseID = c.ID
                                                                                                                                     WHERE cr.RequestID = '" + item.ID + "';").ToList();

                        list.Add(new RequestViewModel()
                        {
                            ID = item.ID,
                            StudentUserName = item.StudentUserName,
                            Type = item.Type,
                            Cause = item.Cause,
                            Date = item.Date,
                            ManagerUserName = item.ManagerUserName,
                            ApprovalHours = item.ApprovalHours,
                            BudgetNumber = item.BudgetNumber,
                            Notes = item.Notes,
                            ManagerSignature = item.ManagerSignature,
                            SignatureDate = item.SignatureDate,
                            CourseRequests = courseRequestsViewModel
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Problem with requests: " + e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Problem with GetRequestByUser function: " + e);
            }
            //add list
            listRequestsViewModel.List = list;
            return listRequestsViewModel;
        }
        public SessionListViewModel GetStudentSessions(string StudentUserName, DateTime date)
        {
            SessionListViewModel sessionListViewModel = new SessionListViewModel();
            List<Session> sessions = this.GetSessions(StudentUserName, date);
            List<SessionViewModel> sessionViewModelList = new List<SessionViewModel>();

            foreach (var item in sessions)
            {
                sessionViewModelList.Add(new SessionViewModel()
                {
                    ID = item.ID,
                    StudentUserName = item.StudentUserName,
                    RefundID = item.RefundID,
                    TeacherUserName = item.TeacherUserName,
                    Date = item.Date,
                    StartHour = item.StartHour,
                    EndHour = item.EndHour,
                    SumHoursPerSession = item.SumHoursPerSession,
                    Details = item.Details,
                    StudentSignature = item.StudentSignature
                });
            }
            sessionListViewModel.List = sessionViewModelList;
            return sessionListViewModel;
        }

        // teacher bussiness
        public RefundListViewModel GetTeacherRefunds(string teacherUserName, DateTime date)
        {
            // create list of refunds instance
            RefundListViewModel listRefundsViewModel = new RefundListViewModel();
            // initialize list of refunds
            List<RefundViewModel> list = new List<RefundViewModel>();

            try
            {
                List<Refund> refunds = this.GetRefundsByMemberID(teacherUserName, date);

                // for each refund get the matching sessions
                foreach (var refund in refunds)
                {
                    try
                    {
                        List<SessionViewModel> sessions = dbContext.Database.SqlQuery<SessionViewModel>(@"SELECT *
                                                                                                        FROM [dbo].[Session]
                                                                                                        WHERE RefundID = '" + refund.ID + "';").ToList();

                        list.Add(new RefundViewModel()
                        {
                            ID = refund.ID,
                            TeacherUserName = refund.TeacherUserName,
                            Date = refund.Date,
                            CourseID = refund.CourseID,
                            LectuerName = refund.LecturerName,
                            ManagerUserName = refund.ManagerUserName,
                            BudgetNumber = refund.BudgetNumber,
                            RefundSessions = sessions
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Problem with refunds: " + e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Problem with GetTeacherRefunds function: " + e);
            }
            //add list
            listRefundsViewModel.List = list;
            return listRefundsViewModel;
        }
    }
}