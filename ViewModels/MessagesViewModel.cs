using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeanReports.ViewModels
{
    public class MessagesViewModel
    {
        public int ID { get; set; }
        public string From { get; set; }
        public string ToUser { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}