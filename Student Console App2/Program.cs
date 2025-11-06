using Microsoft.EntityFrameworkCore;
using Student_Class_Library;
using Microsoft.EntityFrameworkCore.Design;
namespace Student_Console_App2
{
    internal class Program
    {
        static void Main(string[] args)
        {
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


            var student1 = new student
            {
                Name = "Alice Johnson",
                Age = 20,
                Email = "bean"
            };
            var student2 = new student
            {
                Name = "Bob Smith",
                Age = 22,
                Email = "joune"
            };
            var student3 = new student
            {
                Name = "Charlie Brown",
                Age = 19,
                Email = "adasfasdf"
            };
            student1.Courses = new List<Course> { course1, course3 };
            student2.Courses = new List<Course> { course1, course2 };
            student3.Courses = new List<Course> { course2, course3 };
            using (var context = new StudentsContext())
            {
                context.Database.EnsureCreated();
                context.Students.AddRange(student1, student2, student3);
                context.Courses.AddRange(course1, course2, course3);
                context.SaveChanges();
            }
            Console.WriteLine("your data has been saved");
            var students = new List<student>();
            using (var context = new StudentsContext())
            {
                students = context.Students
                    .Include(s => s.Courses)
                    .ToList();
            }
            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}, Age: {student.Age}, Email: {student.Email}");
                foreach (var course in student.Courses)
                {
                    Console.WriteLine($"\tEnrolled in: {course.Name} ({course.department}) - Lecturer: {course.lecturer}");
                }
            }

        }
        
    }
    public class StudentsContext : DbContext
    {
        public DbSet<student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source=C:\\Users\\AdamHoban-STUDENT\\source\\repos\\Lab 6\\Student Console App2\\student.db");

    }
}
