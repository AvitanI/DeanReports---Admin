using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeanReports.Models
{
    public enum MessageType { General, Request, Session, Refund}

    [Table("Messages")]
    public class Messages
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public MessageType Type { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string ToUser { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool? IsSeen { get; set; }
        public DateTime? SeenDate { get; set; }
    }
}