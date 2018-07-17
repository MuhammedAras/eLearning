﻿using Learning.Data;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.Web.Controllers
{
    public class BaseController : ApiController
    {
        private readonly ILearningRepository _repo;
        private ModelFactory _modelFactory;

        public BaseController(ILearningRepository repo)
        {
            _repo = repo;
        }
        public ILearningRepository TheRepository
        {
            get
            {
                return _repo;
            }
            
        }
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(Request);
                }
                return _modelFactory;
                
            }
        }
    }
}
