using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using System.Collections.Generic;

namespace Student_MVC_App.Data
{
    public class StudentsContext : IdentityDbContext<Appusers, IdentityRole<Guid>, Guid>
    {
        public StudentsContext(DbContextOptions<StudentsContext> options)
            : base(options)
        {
        }

        // Domain DbSets
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<project> projects { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CRITICAL: Call base to configure Identity tables
            base.OnModelCreating(modelBuilder);

            // Configure composite key for StudentCourse
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            // Configure relationships for StudentCourse
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many: Appusers ↔ Team
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithMany(u => u.teams)
                .UsingEntity<Dictionary<string, object>>(
                    "UserTeam",
                    j => j.HasOne<Appusers>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                    j => j.HasKey("UserId", "TeamId")
                );

            // One-to-many: Appusers → Project
            modelBuilder.Entity<project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many: Project → Team
            modelBuilder.Entity<Team>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Teams)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required fields and constraints
            modelBuilder.Entity<Appusers>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<project>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Team>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.department)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(c => c.lecturer)
                .IsRequired();
        }
    }
}