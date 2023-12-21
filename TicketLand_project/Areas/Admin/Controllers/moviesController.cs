using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketLand_project.Models;
using PagedList;
using System.IO;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class moviesController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/movies
        public ActionResult Index(int? page)
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

            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo memberID mới có thể phân trang.
            var _movie = (from l in db.movies
                          select l).OrderBy(x => x.movie_id);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 11;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các member được phân trang theo kích thước và số trang.
            return View(_movie.ToPagedList(pageNumber, pageSize));
        }


        public PartialViewResult GetMovie(int? page)
        {
            int pageSize = 10; // Set your desired page size
            int pageNumber = page ?? 1;

            var rooms = db.rooms.ToList().ToPagedList(pageNumber, pageSize);

            return PartialView("Movie_partial", rooms);
        }

        // GET: Admin/movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // GET: Admin/movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/movies/Create
        //[HttpPost]
        //public JsonResult Create([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_poster,movie_actor,movie_director,movie_status,rate,movie_banner")] movy movy)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.movies.Add(movy);
        //        db.SaveChanges();
        //        return Json(new { success = true, message = "Thêm phim thành công" });
        //    }
        //    return Json(new { success = true, message = "Thêm phim thành công" });
        //}

        [HttpPost]
        public JsonResult Create([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_actor,movie_director,movie_status,rate,movie_banner")] movy movy, HttpPostedFileBase movie_poster, HttpPostedFileBase movie_banner)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var check_movies = db.movies.Any(x => x.movie_name == movy.movie_name);
                    if (!check_movies) {
                        // Xử lý trường input kiểu file (poster)
                        if (movie_poster != null && movie_poster.ContentLength > 0)
                        {
                            // Lấy tên tệp tin
                            var posterFileName = Path.GetFileName(movie_poster.FileName);

                            // Tạo đường dẫn tương đối
                            var relativePosterPath = "\\Assets\\img\\home\\poster_film\\" + posterFileName;

                            // Lưu đường dẫn tương đối của tệp tin vào thuộc tính movie_poster của đối tượng movy
                            movy.movie_poster = relativePosterPath;

                            var new_file_name = Guid.NewGuid();
                            var extension = Path.GetExtension(movie_poster.FileName);
                            string newfile = new_file_name + extension;
                            // Lưu tệp tin vào thư mục trên máy chủ (ví dụ: "Assets/img/home/poster_film")
                            string path = Path.Combine(Server.MapPath("~/Assets/img/home/poster_film/"), newfile);
                            movie_poster.SaveAs(path);
                        }

                        // Xử lý trường input kiểu file (banner)
                        if (movie_banner != null && movie_banner.ContentLength > 0)
                        {
                            // Lấy tên tệp tin
                            var bannerFileName = Path.GetFileName(movie_banner.FileName);

                            // Tạo đường dẫn tương đối
                            var relativeBannerPath = "\\Assets\\img\\home\\banner_film\\" + bannerFileName;

                            // Lưu đường dẫn tương đối của tệp tin vào thuộc tính movie_banner của đối tượng movy
                            movy.movie_banner = relativeBannerPath;

                            // Lưu tệp tin vào thư mục trên máy chủ (ví dụ: "Assets/img/home/banner_film")
                            string path_banner = Path.Combine(Server.MapPath("~/Assets/img/home/banner_film/"), bannerFileName);
                            movie_banner.SaveAs(path_banner);

                        }

                        //Check thời gian phim sắp chiếu. Nếu nó > thời gian hiện tại -> lỗi
                        if (movy.movie_release <  DateTime.Today)
                        {
                            return Json(new { success = false, message = "Phim sắp chiếu phải có thời gian lớn hơn thời gian hiện tại" });

                        }
                        else if (movy.movie_release >  DateTime.Today)
                        {
                            movy.movie_status = 2;
                        }
                        else{
                            movy.movie_status = 1;
                        }

                        // Thêm đối tượng movy vào DbContext và lưu thay đổi
                        db.movies.Add(movy);
                        db.SaveChanges();

                        return Json(new { success = true, message = "Thêm phim thành công" });
                    }
                    return Json(new { success = false, message = "Phim đã tồn tại" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"{ex.Message}" });
                }
            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }




        // GET: Admin/movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Admin/movies/Edit/5
        [HttpPost]
        public JsonResult Edit([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_poster,movie_actor,movie_director,movie_status,rate, movie_banner")] movy movy)
        {
            if (ModelState.IsValid)
            {
                //Check thời gian phim sắp chiếu. Nếu nó > thời gian hiện tại -> lỗi
                if (movy.movie_release <  DateTime.Today)
                {
                    return Json(new { success = false, message = "Phim sắp chiếu phải có thời gian lớn hơn thời gian hiện tại" });

                }
                else if (movy.movie_release >  DateTime.Today)
                {
                    movy.movie_status = 2;
                }
                else
                {
                    movy.movie_status = 1;
                }
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Sửa thành công" });
            }
            return Json(new { success = false, message = "Sửa thất bại" });
        }

        // GET: Admin/movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Admin/movies/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            movy movy = db.movies.Find(id);
            if (movy != null)
            {
                db.movies.Remove(movy);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" });
            }
            return Json(new { success = false, message = "Xóa thất bại" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
