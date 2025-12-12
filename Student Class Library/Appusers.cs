using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Student_Class_Library
{
    public class Appusers : IdentityUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public ICollection<project> projects { get; set; } = new List<project>();
        public ICollection<Team> teams { get; set; } = new List<Team>();
    }
}
