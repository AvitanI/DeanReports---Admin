﻿using DeanReports.Models;
using DeanReports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.Services
{
    public class ConverterService
    {
        public static List<ProgramsViewModel> ToProgramsViewModel(List<Programs> model)
        {
            List<ProgramsViewModel> viewModel = new List<ProgramsViewModel>();
            foreach (Programs p in model)
            {
                viewModel.Add(new ProgramsViewModel()
                {
                    ID = p.ID,
                    Name = p.Name
                });
            }
            return viewModel;
        }
        public static List<CourseViewModel> ToCoursesViewModel(List<Course> model)
        {
            List<CourseViewModel> viewModel = new List<CourseViewModel>();
            foreach (Course c in model)
            {
                viewModel.Add(new CourseViewModel()
                {
                    ID = c.ID,
                    Name = c.Name
                });
            }
            return viewModel;
        }
        public static List<CourseRequestViewModel> ToCourseRequestViewModel(List<CourseRequest> model)
        {
            List<CourseRequestViewModel> viewModel = new List<CourseRequestViewModel>();
            foreach (CourseRequest cr in model)
            {
                viewModel.Add(new CourseRequestViewModel()
                {
                    CourseID = cr.CourseID,
                    LecturerName = cr.LecturerName
                });
            }
            return viewModel;
        }
        public static RequestViewModel ToRequestViewModel(Request model, List<CourseRequest> list)
        {
            return new RequestViewModel() 
            {
                ID = model.ID,
                Type = model.Type,
                Cause = model.Cause,
                Date = model.Date,
                CourseRequests = ToCourseRequestViewModel(list)
            };
        }
        public static MemberViewModel ToMemberViewModel(Member model)
        {
            return new MemberViewModel()
            {
                MemberUserName = model.MemberUserName,
                DepartmentID = model.DepartmentID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birth = model.Birth,
                Phone = model.Phone
            };
        }
    }
}