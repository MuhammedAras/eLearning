using Learning.Data;
using Learning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Learning.Web.Models
{
    public class ModelFactory
    {
        private System.Web.Http.Routing.UrlHelper _UrlHelper;
        private ILearningRepository _repo;

        public ModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new System.Web.Http.Routing.UrlHelper(request);
        }
        public StudentBaseModel CreateSummary(Students student)
        {
            return new StudentBaseModel()
            {
                Url = _UrlHelper.Link("Students", new { userName = student.UserName }),
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                EnrollmentsCount = student.Enrollments.Count(),
            };
        }
        public StudentModel Create(Students student)
        {
            return new StudentModel()
            {
                Url = _UrlHelper.Link("Students", new { userName = student.UserName }),
                Id = student.Id,
                Email = student.Email,
                UserName = student.UserName,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                DateOfBirth = student.DateofBirth,
                RegistrationDate = student.RegistrationDate,
                EnrollmentsCount = student.Enrollments.Count(),
                Enrollments = student.Enrollments.Select(e => Create(e))
            };
        }

        public CourseModel Create(Courses course) {
            return new CourseModel() {
                Url = _UrlHelper.Link("Courses", new { id = course.Id }),
                Id = course.Id,
                Name = course.Name,
                Duration = course.Duration,
                Description = course.Description,
                Tutor = Create(course.CourseTutor),
                Subject = Create(course.CourseSubject)

            };
        }
        public SubjectModel Create(Subjects subject)
        {
            return new SubjectModel() {
                Id = subject.Id,
                Name = subject.Name
            };
        }
        public TutorModel Create(Tutors tutor)
        {
            return new TutorModel() {
                Id = tutor.Id,
                Email = tutor.Email,
                UserName = tutor.UserName,
                FirstName = tutor.FirstName,
                LastName = tutor.LastName,
                Gender = tutor.Gender
            };
        }
        public EnrollmentModel Create(Enrollments enrollment)
        {
            return new EnrollmentModel() {
                EnrollmentDate = enrollment.EnrollmentDate,
                Course = Create(enrollment.Course)
            };
        }

        public Courses Parse(CourseModel model)
        {
            try
            {
                var course = new Courses()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Duration = model.Duration,
                    CourseSubject = _repo.GetSubject(model.Subject.Id),
                    CourseTutor = _repo.GetTutor(model.Tutor.Id)
                };

                return course;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}