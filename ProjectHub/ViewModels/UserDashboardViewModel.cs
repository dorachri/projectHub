using ProjectHub.Areas.Identity.Data;
using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.ViewModels
{
    public class UserDashboardViewModel
    {
        public List<ApplicationUser> AllUsers { get; set; }

        public List<Student> Students { get; set; }

        public List<Professor> Professors { get; set; }

        public List<Company> Companies { get; set; }
    }
}
