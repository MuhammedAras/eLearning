using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Learning.Web.Services
{
    public class LearningControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        public LearningControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _config = configuration;
        }
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping(); //Will ignore any controls in same name even if they are in different namepsace

            var routeData = request.GetRouteData();

            var controllerName = routeData.Values["controller"].ToString();

            HttpControllerDescriptor controllerDescriptor;

            if (controllers.TryGetValue(controllerName, out controllerDescriptor))
            {

                var version = "2";

                var versionedControllerName = string.Concat(controllerName, "V", version);

                HttpControllerDescriptor versionedControllerDescriptor;
                if (controllers.TryGetValue(versionedControllerName, out versionedControllerDescriptor))
                {
                    return versionedControllerDescriptor;
                }

                return controllerDescriptor;
            }

            return null;

        }

        // Versiyon bilgisi headerda accept bölümünden adresten alınır.
        //Accept: application/json; version=2
        private string GetVersionFromAcceptHeaderVersion(HttpRequestMessage request)
        {
            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader)
            {
                if (mime.MediaType == "application/json")
                {
                    var version = mime.Parameters
                    .Where(v => v.Name.Equals("version", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (version != null)
                    {
                        return version.Value;
                    }
                    return "1";
                }
            }
            return "1";
        }

        // Versiyon bilgisi header bölümünden adresten alınır.
        /*
         User-Agent:Fiddler
         Accept: application/json
         Content-type:application:json
         X-Learning-Version:2 
        */
        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            const string HEADER_NAME = "X-Learning-Version";

            if (request.Headers.Contains(HEADER_NAME))
            {
                var versionHeader = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "1";
        }

        // Versiyon bilgisi adresten alınır.
        //Örn: http://localhost:{your_port}/api/students/?v=2
        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);

            var version = query["v"];

            if (version != null)
            {
                return version;
            }

            return "1";

        }
    }
}