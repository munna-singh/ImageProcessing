using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MakeOverApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //this is for hotdeals
            config.Routes.MapHttpRoute(
                name: "HotDealsGeneral",
                routeTemplate: "api/HotelDeals/",
                defaults: new { controller = "HotelDeals" }
            );

            //this is for hotdeals
            config.Routes.MapHttpRoute(
                name: "Suggestions",
                routeTemplate: "api/suggest/",
                defaults: new { controller = "Suggest" }
            );

        }
    }
}
