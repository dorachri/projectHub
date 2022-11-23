using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectHub.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class Notification
    {
        [BindNever]
        public int NotificationId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        public bool IsSeen { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string ProjectCode { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
