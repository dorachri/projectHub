using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class Project
    {
        [BindNever]
        public int ProjectId { get; set; }

        [Required]
        [BindNever]
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

        [Required]
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

        [Required]
        [Range(1, 5)]
        public int Status { get; set; }

        public int? StudentId { get; set; }

        public Student Student { get; set; }

        public int? ProfessorId { get; set; }

        public Professor Professor { get; set; }
        
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public List<Notification> Notifications { get; set; }

        public List<StatusLog> StatusLogs { get; set; }
    }
}
