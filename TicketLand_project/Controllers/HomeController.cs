using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketLand_project.Models;

namespace TicketLand_project.Controllers
{

    public class HomeController : Controller
    {

        QUANLYXEMPHIMEntities objModel = new QUANLYXEMPHIMEntities();
        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                var movies = objModel.movies.ToList();
                int numberMoviesEnable = 0;
                foreach (var movie in movies)
                {
                    movie.DurationInMinutes = ConvertTimeToMinutes(movie.movie_duration.ToString());
                    if (movie.movie_status == 1) numberMoviesEnable++;
                }
                ViewBag.numberMoviesEnable = numberMoviesEnable;
                return View(movies);
                //return View(objModel.movies.ToList());
            }
            else if (Session["username"].ToString() == "Phương")
            {
                return View();
            }


            return RedirectToAction("Login");
        }

        public static int ConvertTimeToMinutes(string time)
        {
            TimeSpan timeSpan;
            if (TimeSpan.TryParse(time, out timeSpan))
            {
                int minutes = timeSpan.Hours * 60 + timeSpan.Minutes;
                return minutes;
            }

            // Nếu định dạng thời gian không hợp lệ hoặc không thể chuyển đổi thành TimeSpan, trả về giá trị mặc định hoặc ném ra một ngoại lệ tùy thuộc vào yêu cầu của bạn.
            return 0; // Giá trị mặc định (hoặc bạn có thể trả về một giá trị khác)
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }



        // Chức năng đăng kí
        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(member _user, HttpPostedFileBase imageFile)
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
                    _user.role_id = 2;
                    _user.member_point = 0;
                    objModel.Configuration.ValidateOnSaveEnabled = false;
                    objModel.members.Add(_user);

                    objModel.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Tên người dùng đã tồn tại";
                }

            }
            return View();
        }

        //Tạo MD5
        public static string GetMD5(string str)
        {
            string byte2String = null;
            if (str != null)
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
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
                        Session["idMember"] = data.FirstOrDefault().member_id.ToString();
                        Session["IsLoggedIn"] = "1";
                        Session["Avartar"] = data.FirstOrDefault().member_avatar.ToString();
                        // 2: user, 1: admin
                        if (data.FirstOrDefault().role_id == 2)
                        {
                            return RedirectToAction("Index");
                        }
                        else if (data.FirstOrDefault().role_id == 1)
                        {
                            return RedirectToAction("Index", "manage_members", new { area = "Admin" });
                        }
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

        //Lấy thông tin session để hiển thị ra session storage
        public JsonResult GetUserInfo()
        {
            string username = Session["Username"] as string ?? "Guest";
            string idMember = Session["idMember"] as string ?? "-1";
            string isLogin = Session["IsLoggedIn"] as string ?? "0";
            string avatar = Session["Avartar"] as string ?? "Null";
            return Json(new { Username = username, IdMember = idMember, IsLogin = isLogin, Avatar = avatar }, JsonRequestBehavior.AllowGet);
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

 
        public ActionResult ScrollToPosition()
        {
            return RedirectToAction("Index");
        }


    }
}