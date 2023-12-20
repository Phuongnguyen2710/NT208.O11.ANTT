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

            //routes.MapMvcAttributeRoutes();
            ////Định tuyến vào trang Admin -> quản lý user
            //routes.MapRoute(
            //    name: "Admin_manager",
            //    url: "Admin/manage_members/Index",
            //    defaults: new { controller = "manage_members", action = "Index", id = UrlParameter.Optional }
            //).DataTokens["area"] = "Admin";


            //routes.MapRoute(
            //    name: "Admin_manager_Add",
            //    url: "Admin/manage_members/Create",
            //    defaults: new { controller = "manage_members", action = "Index", id = UrlParameter.Optional }
            //).DataTokens["area"] = "Admin";


            ////Định tuyến vào trang Admin -> quản lý room
            //routes.MapRoute(
            //    name: "Admin_rooms",
            //    url: "Admin/rooms/Index",
            //    defaults: new { controller = "rooms", action = "Index", id = UrlParameter.Optional }
            //).DataTokens["area"] = "Admin";


            //routes.MapRoute(
            //    name: "AdminRoomsCreate",
            //    url: "Admin/rooms/Create",
            //    defaults: new { controller = "rooms", action = "Create" }
            //);

            ////Định tuyến vào trang Admin -> quản lý seat
            //routes.MapRoute(
            //    name: "Admin_seats",
            //    url: "Admin/seats/Index",
            //    defaults: new { controller = "seats", action = "Index", id = UrlParameter.Optional }
            //).DataTokens["area"] = "Admin";


            ////Định tuyến vào trang User -> quản lý Login
            //routes.MapRoute(
            //    name: "Login",
            //    url: "Home/Login",
            //    defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            //);

            ////Định tuyến vào trang User -> quản lý Đăng kí
            //routes.MapRoute(
            //    name: "Register",
            //    url: "Home/Register",
            //    defaults: new { controller = "Home", action = "Register", id = UrlParameter.Optional }
            //);

            ////Định tuyến vào trang User -> quản lý Đăng xuất
            //routes.MapRoute(
            //    name: "Logout",
            //    url: "Home/Logout",
            //    defaults: new { controller = "Home", action = "Logout", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



        }
    }
}
