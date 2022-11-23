using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectHub.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class StatusLog
    {
        [BindNever]
        public int StatusLogId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime EventDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Old status")]
        public int OldStatus { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "New status")]
        public int NewStatus { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
