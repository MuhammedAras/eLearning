using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data.Entities
{
    public class Courses
    {
        public Courses()
        {
            CourseTutor = new Tutors();
            CourseSubject = new Subjects();
            Enrollments = new List<Enrollments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        
        public Tutors CourseTutor { get; set; }
        public Subjects CourseSubject { get; set; }
        public ICollection<Enrollments> Enrollments { get; set; }

    }
}
