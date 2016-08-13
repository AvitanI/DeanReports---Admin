using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class CircleDataViewModel
    {
        public string ID {get;set;}
        public int Value {get;set;}
        public int MaxValue {get;set;}
        public List<string[]> Colors {get;set;}
    }
}