using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectHub.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class Professor
    {
        [BindNever]
        public int ProfessorId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Professor code")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string ProfessorCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Institution { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Department { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string School { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Rank { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public List<Project> Projects { get; set; }
    }
}
