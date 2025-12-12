using System;
using System.Collections.Generic;
using System.Text;

namespace Student_Class_Library
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public int ProjectId { get; set; }
        public project Project { get; set; } = null!;

        public ICollection<Appusers> Members { get; set; } = new List<Appusers>();
    }
}
