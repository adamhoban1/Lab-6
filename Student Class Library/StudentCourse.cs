using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Class_Library
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public users Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
