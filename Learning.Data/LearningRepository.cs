using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learning.Data.Entities;
using System.Diagnostics;

namespace Learning.Data
{
    public class LearningRepository : ILearningRepository
    {
        private LearningContext _ctx;
        public LearningRepository(LearningContext ctx)
        {
            _ctx = ctx;
        }

        public bool CourseExists(int CourseId)
        {
            return _ctx.Courses.Any(e => e.Id == CourseId);
        }

        public bool DeleteCourse(int Id)
        {
            var course = _ctx.Courses.Find(Id);

            if (course != null)
            {
                _ctx.Courses.Remove(course);
                return true;
            }
            return false;
        }

        public bool DeleteStudent(int Id)
        {
            var student = _ctx.Students.Find(Id);
            if (student != null)
            {
                _ctx.Students.Remove(student);
                return true;
            }
            return false;
        }

        public int EnrollStudentInCourse(int StudentId, int CourseId, Enrollments enrolment)
        {
            try
            {
                if (_ctx.Enrollments.Any(e => e.Course.Id == CourseId && e.Student.Id == StudentId))
                {
                    return 2;
                }

                _ctx.Database.ExecuteSqlCommand("INSERT INTO Enrollments VALUES (@p0, @p1, @p2)",
                    enrolment.EnrollmentDate, CourseId.ToString(), StudentId.ToString());

                return 1;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbex)
            {
                foreach (var eve in dbex.EntityValidationErrors)
                {
                    string line = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        line = string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                    }
                }
                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IQueryable<Courses> GetAllCourses()
        {
            return _ctx.Courses
                    .Include("CourseSubject")
                    .Include("CourseTutor")
                    .AsQueryable();
        }

        public IQueryable<Students> GetAllStudentsSummary()
        {
            return _ctx.Students
                    .AsQueryable();
        }

        public IQueryable<Students> GetAllStudentsWithEnrollments()
        {
            return _ctx.Students
                    .Include("Enrollments")
                    .Include("Enrollments.Course")
                     .Include("Enrollments.Course.CourseSubject")
                     .Include("Enrollments.Course.CourseTutor")
                     .AsQueryable();
        }

        public IQueryable<Subjects> GetAllSubject()
        {
            return _ctx.Subjects.AsQueryable();
        }

        public Courses getCourse(int CourseId, bool IncludeEnrollments = true)
        {
            if (IncludeEnrollments)
            {
                return _ctx.Courses
                    .Include("Enrollments")
                   .Include("CourseSubject")
                   .Include("CourseTutor")
                   .Where(c => c.Id == CourseId)
                   .SingleOrDefault();
            }
            else
            {
                return _ctx.Courses
                       .Include("CourseSubject")
                       .Include("CourseTutor")
                       .Where(c => c.Id == CourseId)
                       .SingleOrDefault();
            }
        }

        public IQueryable<Courses> GetCoursesBySubject(int SubjectId)
        {
            return _ctx.Courses
                .Include("CourseSubject")
                .Include("CourseTutor")
                .Where(e => e.CourseSubject.Id == SubjectId)
                .AsQueryable();
        }

        public IQueryable<Students> GetEnrolledStudentsInCourse(int CourseId)
        {
            return _ctx.Students
                .Include("Enrollments")
                .Where(e => e.Enrollments.Any(s => s.Course.Id == CourseId))
                .AsQueryable();
        }

        public Students GetStudents(string UserName)
        {
            return _ctx.Students
                .Include("Enrollments")
                .Where(e => e.UserName == UserName)
                .SingleOrDefault();
        }

        public Students GetStudentsEnrollments(string UserName)
        {
          
            var student = _ctx.Students
                          .Include("Enrollments")
                          .Include("Enrollments.Course")
                          .Include("Enrollments.Course.CourseSubject")
                          .Include("Enrollments.Course.CourseTutor")
                          .Where(s => s.UserName == UserName)
                          .SingleOrDefault();

            return student;
        }

        public Subjects GetSubject(int SubjectId)
        {
            return _ctx.Subjects.Find(SubjectId);
        }

        public Tutors GetTutor(int TutorId)
        {
            return _ctx.Tutors.Find(TutorId);
        }

        public bool Insert(Courses course)
        {
            try
            {
                _ctx.Courses.Add(course);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool Insert(Students student)
        {
            try
            {
                _ctx.Students.Add(student);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool LoginStudent(string UserName, string password)
        {
            var student = _ctx.Students.Where(e=>e.UserName==UserName).SingleOrDefault();

            if (student != null)
            {
                if (student.Password == password)
                    return true;
            }
            return false;
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        public bool Update(Courses originalCourse, Courses updatedCourse)
        {
            _ctx.Entry(originalCourse).CurrentValues.SetValues(updatedCourse);
            originalCourse.CourseSubject = updatedCourse.CourseSubject;
            originalCourse.CourseTutor = updatedCourse.CourseTutor;
            return true;
        }

        public bool Update(Students originalStudents, Students updatedStudent)
        {
            _ctx.Entry(originalStudents).CurrentValues.SetValues(updatedStudent);
            return true;
        }
        
    }
}
