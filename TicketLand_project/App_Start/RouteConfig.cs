using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketLand_project.Areas.Admin;
using TicketLand_project.Models;

namespace TicketLand_project
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //Route của Admin Site

            routes.MapRoute(
                name: "User_manage",
                url: "Admin/manage_members/Index",
                defaults: new { area = "Admin", controller = "manage_members", action = "Index"}
            );

            routes.MapRoute(
                name: "User_New",
                url: "Admin/manage_members/Create",
                defaults: new { area = "Admin", controller = "manage_members", action = "Create"}
            );

            routes.MapRoute(
                name: "Edit_Member",
                url: "Admin/manage_members/Edit/{id}",
                defaults: new { area = "Admin", controller = "manage_members", action = "Edit", id = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "EditSchedule",
                url: "Admin/schedule/Edit/{id}",
                defaults: new { area = "Admin", controller = "schedules", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DetailSchedule",
                url: "Admin/schedule/Detail/{id}",
                defaults: new { area = "Admin", controller = "schedules", action = "Detail", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "AdminBookingDetail",
                url: "Admin/bookings/Booking_Detail/{booking_id}",
                defaults: new { area = "Admin", controller = "bookings", action = "Booking_Detail" }
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