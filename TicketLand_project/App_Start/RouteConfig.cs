using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TicketLand_project
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Create",
            url: "Admin/Room/{roomId}/seats/Create",
            defaults: new { controller = "seats", action = "Create" }
            );

           routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
    name: "ScrollToPosition",
    url: "Home/ScrollToPosition",
    defaults: new { controller = "Home", action = "ScrollToPosition" }
);


        }
    }
}
