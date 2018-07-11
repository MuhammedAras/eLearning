using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data.Entities
{
    public class Subjects
    {
        public Subjects()
        {
            Courses = new List<Courses>();
        }
        public int Id { get; set; }
        public string  Name { get; set; }
        public  ICollection<Courses> Courses { get; set; }
    }
}
