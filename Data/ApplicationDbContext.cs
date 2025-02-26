using Microsoft.EntityFrameworkCore;
using SimpleApp.Models;

namespace SimpleApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<SimpleApp.Models.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
               .HasOne(p => p.User)
               .WithMany(u => u.Projects) // Explicitly define the navigation property
               .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<SimpleApp.Models.Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks) // Explicitly define the navigation property
                .HasForeignKey(t => t.ProjectId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
