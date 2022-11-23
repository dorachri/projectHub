using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectHub.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Type { get; set; }

        public List<SelectListItem> Types { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Privately Held", Text = "Privately Held" },
            new SelectListItem { Value = "Public Company", Text = "Public Company" },
            new SelectListItem { Value = "Partnership", Text = "Partnership" },
        };

        [Required]
        [DataType(DataType.Text)]
        public string Industry { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Specialties { get; set; }

        [DataType(DataType.Url)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string Website { get; set; }

        public ApplicationUser User { get; set; }
    }
}
