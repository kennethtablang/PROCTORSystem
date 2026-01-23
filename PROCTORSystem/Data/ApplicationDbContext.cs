using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROCTORSystem.Models;

namespace ProctorSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Core domain tables
        public DbSet<Student> Students { get; set; }
        public DbSet<RemoteCommand> RemoteCommands { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Example: configure cascade/delete behavior explicitly
            builder.Entity<RemoteCommand>()
                   .HasOne(rc => rc.Student)
                   .WithMany()
                   .HasForeignKey(rc => rc.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AuditLog>()
                   .HasOne(a => a.PerformedBy)
                   .WithMany()
                   .HasForeignKey(a => a.PerformedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes for quick lookups
            builder.Entity<Student>()
                   .HasIndex(s => s.MachineName);

            builder.Entity<Student>()
                   .HasIndex(s => s.IPAddress);
        }
    }
}
