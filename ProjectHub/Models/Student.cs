using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectHub.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Models
{
    public class Student
    {
        [BindNever]
        public int StudentId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Student code")]
        public string StudentCode { get; set; }

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
        public string Degree { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Project Project { get; set; }
    }
}
