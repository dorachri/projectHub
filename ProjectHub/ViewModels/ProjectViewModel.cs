using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string Notes { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Publication date")]
        public DateTime PublicationDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Project url")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string ProjectUrl { get; set; }

        public int Status { get; set; }

        public List<SelectListItem> Statuses { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Not assigned" },
            new SelectListItem { Value = "2", Text = "Assigned" },
            new SelectListItem { Value = "3", Text = "Ongoing" },
            new SelectListItem { Value = "4", Text = "Submitted" },
            new SelectListItem { Value = "5", Text = "Done" },
        };

        public int? StudentId { get; set; }

        public Student Student { get; set; }

        public int? ProfessorId { get; set; }

        public Professor Professor { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
