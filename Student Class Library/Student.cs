using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Student_Class_Library
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }    
        public string Email { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    }

    
}
