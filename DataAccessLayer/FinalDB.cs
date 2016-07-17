using DeanReports.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeanReports.DataAccessLayer
{
    public class FinalDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseDepartments> CourseDepartments { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<ProgramsMembers> ProgramsMembers { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<CourseRequest> CourseRequest { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }
}