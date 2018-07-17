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

        public ModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new System.Web.Http.Routing.UrlHelper(request);
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
    }
}