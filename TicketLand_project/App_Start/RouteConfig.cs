using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketLand_project.Areas.Admin;

namespace TicketLand_project
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();

            //Route của Admin Site
            routes.MapRoute(
                name: "EditSchedule",
                url: "Admin/schedule/Edit/{id}",
                defaults: new { controller = "schedules", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DetailSchedule",
                url: "Admin/schedule/Detail/{id}",
                defaults: new { controller = "schedules", action = "Detail", id = UrlParameter.Optional }
            );
            //Kết thúc


            //Route của User
            //Kết thúc


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}