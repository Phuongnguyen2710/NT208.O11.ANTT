//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.Mvc;
//using TicketLand_project.Models;

//namespace TicketLand_project.Areas.Admin.Controllers
//{
//    public class Home_AdminController : Controller
//    {
//        // Kết nối với database
//        QUANLYXEMPHIMEntities1 objModel = new QUANLYXEMPHIMEntities1();

//        public ActionResult Index()
//        {
//            if (Session["Username"] == null)
//            {
//                RedirectToAction("Login_Admin");
//            }
//            return View();
//        }

//        public ActionResult Login_Admin()
//        {
//            return View();
//        }

//        //create a string MD5
//        public static string GetMD5(string str)
//        {
//            string byte2String = null;
//            if (str != null)
//            {
//                MD5 md5 = new MD5CryptoServiceProvider();
//                byte[] fromData = Encoding.UTF8.GetBytes(str);
//                byte[] targetData = md5.ComputeHash(fromData);

//                for (int i = 0; i < targetData.Length; i++)
//                {
//                    byte2String += targetData[i].ToString("x2");

//                }
//            }
//            return byte2String;
//        }

//        // GET: Admin/Login_admin
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Login_Admin(string username, string password)
//        {
//            if (ModelState.IsValid)
//            {

//                if (username != "" && password != "")
//                {
//                    var f_password = GetMD5(password);
//                    var data = objModel.members.Where(s => s.username.Equals(username) && s.password.Equals(f_password)).ToList();
//                    if (data.Count() > 0)
//                    {
//                        string role = data.FirstOrDefault().ROLE.role_name;

//                        if (role == "admin") // Role ID 0: admin
//                        {
//                            // Thêm session 
//                            Session["Username"] = data.FirstOrDefault().username;
//                            Session["idMember"] = data.FirstOrDefault().member_id;

//                            return RedirectToAction("Index", "manage_members", new { area = "Admin" });
//                        }
//                        else
//                        {
//                            // Báo lỗi nếu không là admin
//                            ViewBag.Message = "Tài khoản của bạn không có quyền admin!!!";
//                        }
//                    }
//                    else if (data.Count() == 0)
//                    {
//                        ViewBag.Message = "Tài khoản admin không hợp lệ";
//                    }
//                }
//                else
//                {
//                    ViewBag.Message = "Vui lòng nhập thông tin tài khoản admin";
//                }
//            }
//            return View("Login_Admin");
//        }
//    }
//}