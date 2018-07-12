using Learning.Data;
using Learning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.Web.Controllers
{
    public class CoursesController : ApiController
    {
        public List<Courses> Get()
        {
            ILearningRepository repository = new LearningRepository(new LearningContext());
            return repository.GetAllCourses().ToList();
        }
        public Courses GetCourse(int id)
        {
            ILearningRepository repository = new LearningRepository(new LearningContext());
            return repository.getCourse(id);
        }
    }
}
