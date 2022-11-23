using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectHub.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class StatusLogsViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public List<StatusLog> StatusLogs { get; set; }

        public List<SelectListItem> Statuses { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Not assigned" },
            new SelectListItem { Value = "2", Text = "Assigned" },
            new SelectListItem { Value = "3", Text = "Ongoing" },
            new SelectListItem { Value = "4", Text = "Submitted" },
            new SelectListItem { Value = "5", Text = "Done" },
        };
    }
}
