using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }

        [DataType(DataType.Text)]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [DataType(DataType.Text)]
        public string Industry { get; set; }

        [Display(Name = "Search range")]
        public int SearchRange { get; set; }

        public List<SelectListItem> SearchRanges { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "Whenever" },
            new SelectListItem { Value = "3", Text = "Last trimester" },
            new SelectListItem { Value = "6", Text = "Last semester" },
            new SelectListItem { Value = "12", Text = "Last year" },
        };

        public bool DefaultSearch { get; set; }
    }
}
