using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TicketLand_project.Models;

namespace TicketLand_project.Controllers
{
    public class HomeController : Controller
    {

        QUANLYXEMPHIMEntities1 objModel = new QUANLYXEMPHIMEntities1();
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dangki
        public ActionResult Dangki()
        {
            return View();
        }



        // Chức năng đăng kí
        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangki(member _user, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                    var check = objModel.members.FirstOrDefault(s => s.username == _user.username);
                    if (check == null)
                    {
                        //Mã hóa mật khẩu
                        _user.password = GetMD5(_user.password);

                        // Xử lý hình ảnh
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            using (var binaryReader = new BinaryReader(imageFile.InputStream))
                            {
                                byte[] imageData = binaryReader.ReadBytes(imageFile.ContentLength);
                                _user.member_avatar = Convert.ToBase64String(imageData);
                            }
                        }
                        _user.role_id = 1;
                        _user.member_point = 0;
                        objModel.Configuration.ValidateOnSaveEnabled = false;
                        objModel.members.Add(_user);

                        objModel.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Tên người dùng đã tồn tại";
                        return View();
                    }

            }
            return View();
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            string byte2String = null;
            if (str !=  null)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);

                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String += targetData[i].ToString("x2");

                }   
            }
            return byte2String;
        }


        //Chức năng đăng nhập
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dangnhap(string username, string password)
        {
            if (ModelState.IsValid)
            {

                if (username != "" && password != "")
                {
                    var f_password = GetMD5(password);
                    var data = objModel.members.Where(s => s.username.Equals(username) && s.password.Equals(f_password)).ToList();
                    if (data.Count() > 0)
                    {
                        //add session
                        Session["Username"] = data.FirstOrDefault().username;
                        Session["idMember"] = data.FirstOrDefault().member_id;
                        return RedirectToAction("Index");
                    }
                    else if (data.Count() == 0)
                    {
                        ViewBag.Message = "Tài khoản không hợp lệ";
                    }
                }
                else
                {
                    ViewBag.Message = "Vui lòng nhập thông tin tài khoản";
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Dangnhap");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}