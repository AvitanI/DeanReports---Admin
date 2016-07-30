using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeanReports.ViewModels
{
    public class MessagesViewModel
    {
        public int ID { get; set; }
        public string From { get; set; }
        [Required]
        public string ToUser { get; set; }
        [Required]
        public string Subject { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string PureContent { get; set; }
        public DateTime Date { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? SeenDate { get; set; }
        public List<MemberViewModel> Members { get; set; } 
    }
}