using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using TicketLand_project.Models;
using TicketLand_project.ViewModels;

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
                        // 2: user, 1: admin
                        if (data.FirstOrDefault().role_id == 2)
                        {
                            var returnUrl = Session["ReturnUrlAfterLogin"] as string;
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                // Xóa đường dẫn đã lưu trong session
                                Session.Remove("ReturnUrlAfterLogin");

                                // Chuyển hướng người dùng trở lại đường dẫn trước đó
                                return Redirect(returnUrl);
                            }
                            else
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
            return Json(new { Username = username, IdMember = idMember, IsLogin = isLogin }, JsonRequestBehavior.AllowGet);
        }

 
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
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

        public ActionResult MovieDetail(int id)
        {
            ViewBag.Message = "Movie Detail";
            
            //string decodedTitle = System.Web.HttpUtility.UrlDecode(title);
            var movieEntity = objModel.movies.FirstOrDefault(m => m.movie_id == id);
            if (movieEntity != null)
            {
                // Chuyển đổi từ Entity sang ViewModel
                var viewModel = new MovieDetailViewModel
                {
                    Id = movieEntity.movie_id,
                    Title = movieEntity.movie_name,
                    Genre = movieEntity.movie_genres,
                    Director = movieEntity.movie_director,
                    Actors = movieEntity.movie_actor,
                    ReleaseDate = (DateTime)movieEntity.movie_release,
                    Description = movieEntity.movie_description,
                    PosterUrl = movieEntity.movie_poster,
                    Trailer = GetDataAfterString(movieEntity.movie_trailer,".be/"),
                    Duration = movieEntity.movie_duration,
                    Format = movieEntity.movie_format,
                    MovieCens = movieEntity.movie_cens,
                    Comments = movieEntity.comments.Select(c => new CommentViewModel
                    {
                        CommentId = c.comment_id,
                        Content = c.content,
                        CommentStar = (float)c.comment_star,
                        CommentDate = (DateTime)c.comment_date,
                        MemberName = c.member.member_name,
                        MemberAvatar = c.member.member_avatar,
                    }).Reverse().ToList(),
                    AverageRating = /*(float)movieEntity.rate,*/
                   (float)Math.Round((double)(movieEntity.comments.Any() ? movieEntity.comments.Average(c => c.comment_star) : 0), 1),
                   
                    Schedules = objModel.schedules.Where(s=>s.movie_id == movieEntity.movie_id).ToList(),
                    Rooms = objModel.rooms.ToList()
                };
                // Thêm các thông tin chi tiết khác của phim
            
                return View(viewModel);
            }
            else {
                return HttpNotFound(); 
            }
                
            // Truyền dữ liệu vào view
        }
        [HttpPost]
        public ActionResult AddComment(int movieId, string content, float commentStar)
        {
            // Lấy thông tin người dùng hiện tại (đã đăng nhập)
            var currentUserId = GetCurrentUserId();

            if (currentUserId == -1)
            {
                // Lưu đường dẫn trước đó vào session để sử dụng sau khi đăng nhập
                Session["ReturnUrlAfterLogin"] = Request.UrlReferrer?.ToString();
                // Xử lý khi người dùng chưa đăng nhập
                return RedirectToAction("Login");
            }

            // Tạo đối tượng Comment
            var newComment = new comment
            {
                movie_id = movieId,
                member_id = currentUserId,
                content = content,
                comment_star = commentStar,
                comment_date = DateTime.Now,
            };
            // Thêm đánh giá và bình luận mới vào cơ sở dữ liệu
            objModel.comments.Add(newComment);
            objModel.SaveChanges();

            // Lấy thông tin phim và cập nhật AverageRating
            var movie = objModel.movies.Find(movieId);

            if (movie != null)
            {
                // Tính toán lại AverageRating dựa trên các đánh giá
                movie.rate = (float)Math.Round((double)(movie.comments.Any() ? movie.comments.Average(c => c.comment_star) : 0), 1);

                // Cập nhật bản ghi trong cơ sở dữ liệu
                objModel.SaveChanges();
            }
            int id = movieId;

            // Chuyển hướng về trang chi tiết phim
            return RedirectToAction("MovieDetail", new { id });
        }
        private string GetDataAfterString(string input, string substring)
        {
            // Kiểm tra xem chuỗi có chứa dấu '=' không
            int index = input.IndexOf(substring);

            if (index != -1)
            {
                // Lấy các ký tự sau dấu '='
                string dataAfterSubstring = input.Substring(index + substring.Length);
                return dataAfterSubstring;
            }

            // Trả về null nếu không tìm thấy trong chuỗi
            return null;
        }
        private int GetCurrentUserId()
        {
            // Implement logic để lấy MemberId từ session hoặc cookie
            // ...
            if (Session["idMember"] != null)
            {
                // Ép kiểu Session["idMember"] về kiểu int
                if (int.TryParse(Session["idMember"].ToString(), out int memberId))
                {
                    Debug.WriteLine($"Current User ID: {memberId}");
                    // Trả về memberId
                    return memberId;
                }
            }

            Debug.WriteLine("Session 'idMember' not found or cannot be parsed as int.");
            return 0;
        }
        [HttpGet]
        public JsonResult GetDates(int roomNumber)
        {
            // Lấy danh sách các ngày chiếu từ cơ sở dữ liệu dựa trên phòng
            //var dateString = objModel.schedule.Value.ToString("yyyy-MM-dd");
            var dates = objModel.schedules
                .Where(s => s.room_id == roomNumber)
                .Select(s => s.show_date)
                .Distinct()
                .ToList();
            var formattedDates = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();
            return Json(formattedDates, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShowtimes(int roomNumber, DateTime date)
        {
            var targetDate = date.Date;
            // Lấy danh sách thời gian chiếu từ cơ sở dữ liệu dựa trên phòng và ngày
            var rawShowtimes = objModel.schedules
                .Where(s => s.room_id == roomNumber && DbFunctions.TruncateTime(s.show_date) == targetDate)
                .Select(s => new { StartTime = s.time_start, EndTime = s.time_end })
                .ToList();

            var showtimes = rawShowtimes
             .Where(s => s.StartTime != null && s.EndTime != null)
             .Select(s => new
             {
                 StartTime = ((TimeSpan)s.StartTime).ToString(@"hh\:mm"),
                 EndTime = ((TimeSpan)s.EndTime).ToString(@"hh\:mm")
             })
             .ToList();

            return Json(showtimes, JsonRequestBehavior.AllowGet);
        }
    }
        
}
        
 