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

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });


            //Route của Admin Site
            //Member-Manage
            routes.MapRoute(
                name: "User_manage",
                url: "Admin/manage_members/Index",
                defaults: new { area = "Admin", controller = "manage_members", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Member",
                url: "Admin/manage_members/Create",
                defaults: new { area = "Admin", controller = "manage_members", action = "Create" }
            );

            routes.MapRoute(
                name: "Edit_Member",
                url: "Admin/manage_members/Edit/{id}",
                defaults: new { area = "Admin", controller = "manage_members", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Detail_Member",
                url: "Admin/manage_members/Details/{id}",
                defaults: new { area = "Admin", controller = "manage_members", action = "Details", id = UrlParameter.Optional }
            );
            //Kết thúc



            //Manage-Rooms
            routes.MapRoute(
                name: "Room_manage",
                url: "Admin/rooms/Index",
                defaults: new { area = "Admin", controller = "rooms", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Room",
                url: "Admin/rooms/Create",
                defaults: new { area = "Admin", controller = "rooms", action = "Create" }
            );

            routes.MapRoute(
                name: "Edit_Room",
                url: "Admin/rooms/Edit/{id}",
                defaults: new { area = "Admin", controller = "rooms", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Delete_Room",
                url: "Admin/rooms/Delete/{id}",
                defaults: new { area = "Admin", controller = "rooms", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Detail_Room",
                url: "Admin/rooms/Details/{id}",
                defaults: new { area = "Admin", controller = "rooms", action = "Delete", id = UrlParameter.Optional }
            );
            //Kết thúc



            //Manage-Seat
            routes.MapRoute(
                name: "Seats_manage",
                url: "Admin/seats/Index",
                defaults: new { area = "Admin", controller = "seats", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Seat",
                url: "Admin/seats/Create",
                defaults: new { area = "Admin", controller = "seats", action = "Create" }
            );

            routes.MapRoute(
                name: "Edit_Seat",
                url: "Admin/seats/Edit/{id}",
                defaults: new { area = "Admin", controller = "seats", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Delete_Seat",
                url: "Admin/seats/Delete/{id}",
                defaults: new { area = "Admin", controller = "seats", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Details_Seat",
                url: "Admin/seats/Details/{id}",
                defaults: new { area = "Admin", controller = "seats", action = "Details", id = UrlParameter.Optional }
            );
            //Kết thúc



            //Manage-Movie
            routes.MapRoute(
                name: "Movie_manage",
                url: "Admin/movies/Index",
                defaults: new { area = "Admin", controller = "movies", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Movie",
                url: "Admin/movies/Create",
                defaults: new { area = "Admin", controller = "movies", action = "Create" }
            );

            routes.MapRoute(
                name: "Edit_Movie",
                url: "Admin/movies/Edit/{id}",
                defaults: new { area = "Admin", controller = "movies", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Delete_Movie",
                url: "Admin/movies/Delete/{id}",
                defaults: new { area = "Admin", controller = "movies", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Details_Movie",
                url: "Admin/movies/Details/{id}",
                defaults: new { area = "Admin", controller = "movies", action = "Details", id = UrlParameter.Optional }
            );
            //Kết thúc




            //Manage-News
            routes.MapRoute(
                name: "News_manage",
                url: "Admin/news/Index",
                defaults: new { area = "Admin", controller = "news", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_News",
                url: "Admin/news/Create",
                defaults: new { area = "Admin", controller = "news", action = "Create" }
            );

            routes.MapRoute(
                name: "Edit_News",
                url: "Admin/news/Edit/{id}",
                defaults: new { area = "Admin", controller = "news", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Delete_News",
                url: "Admin/news/Delete/{id}",
                defaults: new { area = "Admin", controller = "news", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Details_News",
                url: "Admin/news/Details/{id}",
                defaults: new { area = "Admin", controller = "news", action = "Details", id = UrlParameter.Optional }
            );
            //Kết thúc


            //Manage-Schedule
            routes.MapRoute(
                name: "Schedule_manage",
                url: "Admin/schedules/Index",
                defaults: new { area = "Admin", controller = "schedules", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Schedule",
                url: "Admin/schedules/Create",
                defaults: new { area = "Admin", controller = "schedules", action = "Create" }
            );


            routes.MapRoute(
                name: "Delete_Schedule",
                url: "Admin/schedules/Delete/{id}",
                defaults: new { area = "Admin", controller = "schedules", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "EditSchedule",
              url: "Admin/schedules/Edit/{id}",
              defaults: new { area = "Admin", controller = "schedules", action = "Edit", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "DetailSchedule",
                url: "Admin/schedules/Detail/{id}",
                defaults: new { area = "Admin", controller = "schedules", action = "Detail", id = UrlParameter.Optional }
            );
            //Kết thúc


            //Manage-Booking
            routes.MapRoute(
                name: "Booking_manage",
                url: "Admin/bookings/Index",
                defaults: new { area = "Admin", controller = "bookings", action = "Index" }
            );

            routes.MapRoute(
                name: "Create_Booking",
                url: "Admin/bookings/Create",
                defaults: new { area = "Admin", controller = "bookings", action = "Create" }
            );


            routes.MapRoute(
                name: "Delete_Booking",
                url: "Admin/booking/Delete/{id}",
                defaults: new { area = "Admin", controller = "bookings", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Edit_Booking",
              url: "Admin/booking/Edit/{id}",
              defaults: new { area = "Admin", controller = "bookings", action = "Edit", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "AdminBookingDetail",
                url: "Admin/bookings/Booking_Detail/{booking_id}",
                defaults: new { area = "Admin", controller = "bookings", action = "Booking_Detail" }
            );

            //Kết thúc


            //Manage-Analyze
            routes.MapRoute(
                name: "Analyzie",
                url: "Admin/Dashboard/Index",
                defaults: new { area = "Admin", controller = "Dashboard", action = "Index" }
            );
            //Kết thúc



            //Login
            routes.MapRoute(
                name: "Login",
                url: "Home/Login",
                defaults: new { controller = "Home", action = "Login" }
            );
            //Kết thúc


            //Register
            routes.MapRoute(
                name: "Register",
                url: "Home/Register",
                defaults: new { controller = "Home", action = "Register" }
            );
            //Kết thúc


            //Logout
            routes.MapRoute(
                name: "Logout",
                url: "Home/Logout",
                defaults: new { controller = "Home", action = "Logout" }
            );
            //Kết thúc



            //Route của User



            //Kết thúc

            routes.MapRoute(
                name: "detail",
                url: "{alias}-p{id}",
                defaults: new { controller = "Home", action = "MovieDetail", id = UrlParameter.Optional }
            );

            ////Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



        }
    }
}