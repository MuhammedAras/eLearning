using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Learning.Data;
using Learning.Data.Entities;
using System.Web.Http.Routing;
using Learning.Web.Models;
using Learning.Web.Filters;

namespace Learning.Web.Controllers
{
    public class StudentsV2Controller : BaseController
    {
        public StudentsV2Controller(ILearningRepository repo) : base(repo)
        {
        }
        public IEnumerable<StudentV2BaseModel> Get(int page = 0, int pageSize = 10)
        {
            IQueryable<Students> query;

            query = TheRepository.GetAllStudentsWithEnrollments().OrderBy(c => c.LastName);

            var totalCount = query.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 0 ? urlHelper.Link("Students", new { page = page - 1, pageSize = pageSize }):"";
            var nextLink = page < totalPages ? urlHelper.Link("Students",new { page=page++, pageSize = pageSize }) : "";

            var paginationHeader = new {
                TotalCount=totalCount,
                TotalPages=totalPages,
                PrevPageLink=prevLink,
                NextPageLink=nextLink
            };
            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination",Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            var results = query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(s => TheModelFactory.CreateV2Summary(s));
            return results;
        }
    }
}
