using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectHub.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class StudentViewModel
    {
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

        public List<SelectListItem> Degrees { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Undergraduate", Text = "Undergraduate" },
            new SelectListItem { Value = "Postgraduate", Text = "Postgraduate" },
        };

        public ApplicationUser User { get; set; }
    }
}
