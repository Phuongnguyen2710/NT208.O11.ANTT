using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class Dashborad : Controller
    {
        // Kết nối với database
        QUANLYXEMPHIMEntities objModel = new QUANLYXEMPHIMEntities();

        public ActionResult Index()
        {
            var username = Session["Username"] as string;
            var idMember = Session["idMember"] as string;
            if (username != "phuong2003" || idMember != "1")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


    }
}