using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Learning.Data;
using Learning.Web.Models;
using Learning.Data.Entities;

namespace Learning.Web.Controllers
{
    public class EnrollmentsController : BaseController
    {
        public EnrollmentsController(ILearningRepository repo) : base(repo)
        {
        }

        public IEnumerable<StudentBaseModel> Get(int courseId, int page = 0, int pageSize = 10)
        {
            IQueryable<Students> query;
            query = TheRepository.GetEnrolledStudentsInCourse(courseId).OrderBy(s => s.LastName);

            var totalCount = query.Count();
            System.Web.HttpContext.Current.Response.Headers.Add("X-InlineCount", totalCount.ToString());

            var result = query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(s => TheModelFactory.CreateSummary(s));

            return result;
        }
    
        public HttpResponseMessage Post(int courseId, [FromUri]string userName, [FromBody] Enrollments enrollment)
        {
            try
            {
                if (!TheRepository.CourseExists(courseId))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Course");

                var student = TheRepository.GetStudents(userName);
                if (student == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not find Student");

                var result = TheRepository.EnrollStudentInCourse(student.Id, courseId, enrollment);

                if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else if (result == 2)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student already enrolled in this course");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest,ex);
            }
            
        }
    }

}

