using System;

namespace Student_Class_Library
{
    public class StudentCourse
    {
        public Guid StudentId { get; set; }  // Changed from int to Guid
        public Appusers Student { get; set; }
        
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
