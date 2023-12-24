using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index()
        {
            if (Session["Username"] != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://apitikketland.azurewebsites.net/api/");
                    HttpResponseMessage response = await client.GetAsync("movies");

                    if (response.IsSuccessStatusCode)
                    {
                        var movies = await response.Content.ReadAsAsync<List<movy>>();
                        foreach (movy movie in movies)
                        {
                            movie.DurationInMinutes = ConvertTimeToMinutes(movie.movie_duration.ToString());
                        }
                        return View(movies);
                    }
                    else
                    {
                        // Xử lý khi không nhận được phản hồi thành công từ API
                        // Ví dụ: ghi log, hiển thị thông báo lỗi, vv.
                        return View("Error");
                    }
                }
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

        //public ActionResult GetMovieTimes(string movieName, TimeSpan scheduleDate)
        //{
        //    if (scheduleDate != null)
        //    {
        //        // Sử dụng Entity Framework để truy vấn dữ liệu từ cơ sở dữ liệu
        //        Debug.WriteLine("movieName: " + movieName);
        //        Debug.WriteLine("scheduleDate: " + scheduleDate);

        //        //var movie = objModel.movies.FirstOrDefault(m => m.movie_name == movieName);
        //        //Debug.WriteLine("movie: " + movie.movie_id);
        //        var movie_id = 28;
        //        var schedule = objModel.schedules.Where(s => s.movie_id == movie_id && s.schedule_date_start == scheduleDate).ToList();


        //        Debug.WriteLine("movie: " + schedule);
        //        // Tạo danh sách các thời gian xuất chiếu để truyền về cho client
        //        var movieTimes = schedule.Select(s => s.time_start).ToList();

        //        return Json(movieTimes, JsonRequestBehavior.AllowGet);
        //    }
        //    else {
        //        Debug.WriteLine("Không có lịch chiếu");
        //        return View();
        //    }

        //}

        public async Task<ActionResult> GetMovieTimes(string movieName, DateTime scheduleDate)
        {
            if (scheduleDate != null)
            {
                // Gọi API và lấy dữ liệu trả về
                var apiUrl = "https://apitikketland.azurewebsites.net/api/schedules";
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(apiUrl);
                var responseData = await response.Content.ReadAsStringAsync();

                var apiUrl_2 = "https://apitikketland.azurewebsites.net/api/movies";
                var httpClient2 = new HttpClient();
                var response2 = await httpClient2.GetAsync(apiUrl_2);
                var responseData2 = await response2.Content.ReadAsStringAsync();
                
                var movies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<movy>>(responseData2);
                // Chuyển đổi dữ liệu từ JSON sang danh sách lịch chiếu
                var schedules = Newtonsoft.Json.JsonConvert.DeserializeObject<List<schedule>>(responseData);
                Debug.WriteLine("movies: " + movieName);
                
                var filterMovies = movies.Where(m => m.movie_name == movieName);
                Debug.WriteLine("movies: " + filterMovies);
                int movie_id = filterMovies.Select(m => m.movie_id).FirstOrDefault();

                Debug.WriteLine("movies: " + movie_id);
                // Lọc danh sách lịch chiếu theo movie_id và schedule_date
                var filteredSchedules = schedules.Where(s => s.movie_id == movie_id && s.show_date == scheduleDate);

                // Lấy danh sách các time_start
                var movieTimes = filteredSchedules.Select(s => s.time_start).ToList();

                // In danh sách time_start ra Output Debug
                foreach (var time in movieTimes)
                {
                    Debug.WriteLine("time_start: " + time);
                }

                return Json(movieTimes, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Debug.WriteLine("Không có lịch chiếu");
                return View();
            }
        }

        public class News
        {
            public int news_id { get; set; }
            public int movie_id { get; set; }
            public string news_title { get; set; }
            public string news_content { get; set; }
            public string news_img { get; set; }
            public DateTime news_release { get; set; }
        }

        public async Task<ActionResult> GetNews()
        {
            // Gọi API và lấy dữ liệu trả về
            var apiUrl = "https://apitikketland.azurewebsites.net/api/news";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl);
            var responseData = await response.Content.ReadAsStringAsync();

            // Chuyển đổi dữ liệu từ chuỗi JSON sang đối tượng C#
            var newsList = JsonConvert.DeserializeObject<List<News>>(responseData);

            // Lọc ra những news có news_release cách ngày hiện tại không quá 10 ngày
            var filteredNewsList = new List<News>();

            foreach (var news in newsList)
            {
                DateTime now = DateTime.Now;
                DateTime newsRelease = news.news_release; // hoặc news.news_release.GetValueOrDefault()
                TimeSpan timeDiff = now - newsRelease;

                double daysDiff = timeDiff.TotalDays;
                if (daysDiff <= 10)
                {
                    filteredNewsList.Add(news);
                }
            }

            return Json(filteredNewsList, JsonRequestBehavior.AllowGet);
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