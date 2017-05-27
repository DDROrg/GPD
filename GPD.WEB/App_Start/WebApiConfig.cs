using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace GPD.WEB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Add Project",
                "api/{partnerName}/{controller}",
                new { controller = "Project", action = "AddProject" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            config.Routes.MapHttpRoute(
                "Get Projects List",
                "api/{partnerName}/{controller}/List/{pageSize}/{pageIndex}",
                new { controller = "Project", action = "GetProjectsList", pageSize = RouteParameter.Optional, pageIndex = RouteParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

            config.Routes.MapHttpRoute(
                "Get Project Details",
                "api/{partnerName}/{controller}/{projectId}",
                new { controller = "Project", action = "GetProjectDetails" },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );
        }
    }
}
