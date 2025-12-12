using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Class_Library
{
    public class project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public Guid OwnerId { get; set; }  // Changed from int to Guid
        public Appusers Owner { get; set; } = null!; // Required

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
