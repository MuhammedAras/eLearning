using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.Web.Controllers
{
    public class CoursesController : BaseController
    {
        public CoursesController(ILearningRepository repo) : base(repo)
        {
        }

        public IEnumerable<CourseModel> Get()
        {
            IQueryable<Courses> query;
            query = TheRepository.GetAllCourses();

            var results = query
            .ToList()
            .Select(s => TheModelFactory.Create(s));

            return results;
        }
        public HttpResponseMessage GetCourse(int id)
        {
            try
            {
                var course = TheRepository.getCourse(id);
                if (course != null)
                    return Request.CreateResponse(HttpStatusCode.OK,TheModelFactory.Create(course));
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
