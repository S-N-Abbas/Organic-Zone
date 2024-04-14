using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organic_Zone.Models
{
    public class FAQ
    {
        [Key]
        public int FAQId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Question { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Answer { get; set; }
    }
}