using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using Microsoft.EntityFrameworkCore.Design;

namespace Student_MVC_App.data
{
    public class StudentsContext : DbContext
    {
        public StudentsContext(DbContextOptions<StudentsContext> options) : base(options) { }

        public DbSet<student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many join table once and explicitly set composite key
            modelBuilder.Entity<student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    right => right.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                    left => left.HasOne<student>().WithMany().HasForeignKey("StudentId"),
                    join => join.HasKey("StudentId", "CourseId")
                );

            // Property configurations for student
            modelBuilder.Entity<student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<student>()
                .Property(s => s.Age);

            // Property configurations for Course
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

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Calculus I", department = "Mathematics", lecturer = "Dr. Smith" },
                new Course { Id = 2, Name = "Introduction to Programming", department = "Computer Science", lecturer = "Prof. Chen" },
                new Course { Id = 3, Name = "English Literature", department = "Humanities", lecturer = "Dr. Brown" }
            );

            // Seed Students
            modelBuilder.Entity<student>().HasData(
                new student { Id = 1, Name = "Alice Johnson", Age = 20, Email = "alice@example.com" },
                new student { Id = 2, Name = "Bob Lee", Age = 22, Email = "bob@example.com" },
                new student { Id = 3, Name = "Carol Nguyen", Age = 19, Email = "carol@example.com" },
                new student { Id = 4, Name = "David Kim", Age = 21, Email = "david@example.com" }
            );

            // Seed many-to-many join table (shadow entity "StudentCourse")
            // Use the non-generic Entity(string) overload so the compiler selects the correct overload.
            modelBuilder.Entity("StudentCourse").HasData(
                // Alice: Calculus I, Introduction to Programming
                new { StudentId = 1, CourseId = 1 },
                new { StudentId = 1, CourseId = 2 },

                // Bob: Introduction to Programming
                new { StudentId = 2, CourseId = 2 },

                // Carol: English Literature
                new { StudentId = 3, CourseId = 3 },

                // David: Calculus I, English Literature
                new { StudentId = 4, CourseId = 1 },
                new { StudentId = 4, CourseId = 3 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

