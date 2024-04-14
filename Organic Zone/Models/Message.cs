using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organic_Zone.Models
{
    public class Message
    {
        [Key]
        [Display(Name = "#")]
        public int MessageId { get; set; }

        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        [MaxLength(200)]
        [Display(Name = "Message")]
        public string MessageText { get; set; }
    }
}