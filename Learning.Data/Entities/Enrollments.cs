using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data.Entities
{
    public class Enrollments
    {
        public Enrollments()
        {
            Student = new Students();
            Course = new Courses();
        }
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public  Students Student { get; set; }
        public  Courses Course { get; set; }

    }
}
