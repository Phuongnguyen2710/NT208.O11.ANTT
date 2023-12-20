using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            // Đếm số lượng admin
            int adminCount = db.members.Count(a => a.member_id == 1);

            // Đếm số lượng user
            int userCount = db.members.Count(u => u.member_id != 1);

            // Truyền số liệu đếm vào View
            ViewBag.AdminCount = adminCount;
            ViewBag.UserCount = userCount;

            return View();
        }
    }
}