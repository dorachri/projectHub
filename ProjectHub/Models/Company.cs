using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectHub.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class Company
    {
        [BindNever]
        public int CompanyId { get; set; }

        [BindNever]
        [Display(Name = "Company code")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string CompanyCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Industry { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Specialties { get; set; }

        [DataType(DataType.Url)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string Website { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public List<Project> Projects { get; set; }
    }
}
