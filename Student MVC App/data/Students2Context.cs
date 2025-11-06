using Microsoft.EntityFrameworkCore;
using Student_Class_Library;

namespace Student_MVC_App.data
{
    public class StudentsContext : DbContext
    {
        public DbSet<student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                    j => j.HasOne<student>().WithMany().HasForeignKey("StudentId"));

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    j => j.HasOne<student>().WithMany().HasForeignKey("StudentId"),
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CourseId"));
            modelBuilder.Entity<student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<student>()
                .Property(s => s.Age)
                .IsRequired();
            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Course>()
                .Property(c => c.department)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Course>()
                .Property(c => c.lecturer)
                .IsRequired()
                .HasMaxLength(100);//f
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source=C:\\Users\\AdamHoban-STUDENT\\source\\repos\\Lab 6\\Student Console App2\\student2.db");

    }
}
