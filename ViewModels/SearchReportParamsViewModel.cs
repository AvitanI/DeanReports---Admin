using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class SearchReportParamsViewModel
    {
        public static int Day = 1;

        public int Type { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ExportTo { get; set; }
        public string GetReportName
        {
            get
            {
                string name = "";

                if (this.Type == 1)
                {
                    name = "charge";
                }
                if (this.Type == 2)
                {
                    name = "refund";
                }
                if (this.Type == 3)
                {
                    name = "user";
                }
                return name;
            }
        }
    }
}