using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectHub.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class ProfessorViewModel
    {
        public int ProfessorId { get; set; }

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

        public List<SelectListItem> Ranks { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Professor", Text = "Professor" },
            new SelectListItem { Value = "Assistant Professor", Text = "Assistant Professor" },
            new SelectListItem { Value = "Associate Professor", Text = "Associate Professor" },
        };

        public ApplicationUser User { get; set; }
    }
}
