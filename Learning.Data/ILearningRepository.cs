using Learning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data
{
    public interface ILearningRepository
    {
        IQueryable<Subjects> GetAllSubject();
        Subjects GetSubject(int SubjectId);

        IQueryable<Courses> GetCoursesBySubject(int SubjectId);
        IQueryable<Courses> GetAllCourses();
        Courses getCourse(int CourseId, bool IncludeEnrollments = true);
        bool CourseExists(int CourseId);

        IQueryable<Students> GetAllStudentsWithEnrollments();
        IQueryable<Students> GetAllStudentsSummary();

        IQueryable<Students> GetEnrolledStudentsInCourse(int CourseId);
        Students GetStudentsEnrollments(string UserName);
        Students GetStudents(string UserName);

        Tutors GetTutor(int TutorId);

        bool LoginStudent(string UserName, string password);
        bool Insert(Students student);
        bool Update(Students originalStudents, Students updatedStudent);
        bool DeleteStudent(int Id);

        int EnrollStudentInCourse(int StudentId, int CourseId, Enrollments enrollment);
        bool Insert(Courses course);
        bool Update(Courses originalCourse, Courses updatedCourse);
        bool DeleteCourse(int Id);

        bool SaveAll();
    }
}
