using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectHub.Areas.Identity.Data;

namespace ProjectHub.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Professor> Professors { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<StatusLog> StatusLogs { get; set; }

    }
}
