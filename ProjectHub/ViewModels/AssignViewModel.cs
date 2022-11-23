using System.ComponentModel.DataAnnotations;

namespace ProjectHub.ViewModels
{
    public class AssignViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Student code")]
        public string StudentCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }
    }
}
