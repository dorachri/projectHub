using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectHub.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First name")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-")]
        public string LastName { get; set; }

        [Required]
        public bool Inactive { get; set; }
    }
}
