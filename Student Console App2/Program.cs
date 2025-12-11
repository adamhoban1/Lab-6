using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using Microsoft.EntityFrameworkCore.Design;
namespace Student_Console_App2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new StudentsContext();
            var course1 = new Course
            {
                Name = "Database Systems",
                department = "Computer Science",
                lecturer = "Dr. Smith"
            };
            var course2 = new Course
            {
                Name = "Operating Systems",
                department = "Computer Science",
                lecturer = "Prof. Johnson"
            };
            var course3 = new Course
            {
                Name = "Calculus I",
                department = "Mathematics",
                lecturer = "Dr. Brown"
            };


            var student1 = new Student
            {
                Name = "Alice Johnson",
                Age = 20,
                Email = "bean"
            };
            var student2 = new Student
            {
                Name = "Bob Smith",
                Age = 22,
                Email = "joune"
            };
            var student3 = new Student
            {
                Name = "Charlie Brown",
                Age = 19,
                Email = "adasfasdf"
            };


            student1.StudentCourses = new List<StudentCourse>
               {
                   new StudentCourse { Student = student1, Course = course1 },
                   new StudentCourse { Student = student1, Course = course3 }
               };
            student2.StudentCourses = new List<StudentCourse>
               {
                   new StudentCourse { Student = student2, Course = course1 },
                   new StudentCourse { Student = student2, Course = course2 }
               };
            student3.StudentCourses = new List<StudentCourse>
               {
                   new StudentCourse { Student = student3, Course = course2 },
                   new StudentCourse { Student = student3, Course = course3 }
               };

            db.AddRange(student1, student2, student3, course1, course2, course3);
            db.SaveChanges();
            Console.WriteLine("save data");
            

        }
        public class StudentsContext : DbContext
        {
            public DbSet<Student> Students { get; set; }
            public DbSet<Course> Courses { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder options) =>
                options.UseSqlite($"Data Source=C:\\Users\\joemh\\source\\repos\\adamhoban1\\lab-6\\student Console App2\\student.db");

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
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
            }
        }
    }
}
