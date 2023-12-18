using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TicketLand_project
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Khởi tạo Session với giá trị mặc định, ví dụ: "Guest"
            Session["Username"] = "Guest";
            Session["idMember"] = "-1";
        }


    }
}
