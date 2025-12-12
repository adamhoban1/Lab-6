using Microsoft.EntityFrameworkCore;
using Student_Class_Library; // Your class library namespace

namespace Student_MVC_App.Data
{
    public class StudentsContext : DbContext
    {
        public StudentsContext(DbContextOptions<StudentsContext> options)
            : base(options)
        {
        }

        public DbSet<users> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many join entity
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            // Property constraints for Student
            modelBuilder.Entity<users>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<users>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<users>()
                .Property(s => s.Age)
                .IsRequired();

            // Property constraints for Course
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
                .HasMaxLength(100);

            // --- Seed data ---

            // Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Calculus I", department = "Mathematics", lecturer = "Dr. Smith" },
                new Course { Id = 2, Name = "Introduction to Programming", department = "Computer Science", lecturer = "Prof. Chen" },
                new Course { Id = 3, Name = "English Literature", department = "Humanities", lecturer = "Dr. Brown" }
            );

            // Students
            modelBuilder.Entity<users>().HasData(
                new users { Id = 1, Name = "Alice Johnson", Age = 20, Email = "alice@example.com" },
                new users { Id = 2, Name = "Bob Lee", Age = 22, Email = "bob@example.com" },
                new users { Id = 3, Name = "Carol Nguyen", Age = 19, Email = "carol@example.com" },
                new users { Id = 4, Name = "David Kim", Age = 21, Email = "david@example.com" }
            );

            // Join table StudentCourse
            modelBuilder.Entity<StudentCourse>().HasData(
                new StudentCourse { StudentId = 1, CourseId = 1 },
                new StudentCourse { StudentId = 1, CourseId = 2 },
                new StudentCourse { StudentId = 2, CourseId = 2 },
                new StudentCourse { StudentId = 3, CourseId = 3 },
                new StudentCourse { StudentId = 4, CourseId = 1 },
                new StudentCourse { StudentId = 4, CourseId = 3 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
