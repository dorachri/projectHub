using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User type")]
        public string UserType { get; set; }

        public List<SelectListItem> UserTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Student", Text = "Student" },
            new SelectListItem { Value = "Professor", Text = "Professor" },
            new SelectListItem { Value = "Company", Text = "Company" },
            new SelectListItem { Value = "Other", Text = "Other" },
        };

        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
}
