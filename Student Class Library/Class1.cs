using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace Student_Class_Library
{
    public class student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }    
        public string Email { get; set; }

        public ICollection<Course> Courses { get; set; }

    }

    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string department { get; set; }
        public string lecturer { get; set; }

        public ICollection<student> Students { get; set; }
    }



}
