using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Student_Class_Library
{
    public class Appusers : IdentityUser<Guid>
    {
        // Don't declare Id - it's inherited as Guid from IdentityUser<Guid>
        
        public string Name { get; set; }
        public int Age { get; set; }
        // Email is already in IdentityUser

        public ICollection<project> projects { get; set; } = new List<project>();
        public ICollection<Team> teams { get; set; } = new List<Team>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
