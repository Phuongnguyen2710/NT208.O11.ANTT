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
            var username = Session["Username"] as string;
            var idMember = Session["idMember"] as string;
            int Idmember;

            // Covert sang int
            int.TryParse(idMember, out Idmember);

            var member = db.members.SingleOrDefault(m => m.member_id == Idmember);

            if (member == null || member.role_id == 2)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            // Đếm số lượng admin
            int adminCount = db.members.Count(a => a.role_id == 1);

            // Đếm số lượng user
            int userCount = db.members.Count(u => u.role_id != 1);

            // Truyền số liệu đếm vào View
            ViewBag.AdminCount = adminCount;
            ViewBag.UserCount = userCount;


            // Đếm số lượng phim đang chiếu (giả sử có một trường status để biểu thị tình trạng)
            int nowShowingCount = db.movies.Count(m => m.movie_status == 1);

            // Đếm số lượng phim sắp chiếu
            int upcomingCount = db.movies.Count(m => m.movie_status == 2);

            // Gán kết quả vào ViewBag
            ViewBag.NowShowingCount = nowShowingCount;
            ViewBag.UpcomingCount = upcomingCount;

            int roomCount = db.rooms.Count();
            ViewBag.RoomCount = roomCount;

            return View();
        }
    }
}