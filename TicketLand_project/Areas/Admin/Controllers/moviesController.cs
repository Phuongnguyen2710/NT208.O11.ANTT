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

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class moviesController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/movies
        public ActionResult Index(int? page)
        {
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo memberID mới có thể phân trang.
            var _movie = (from l in db.movies
                          select l).OrderBy(x => x.movie_id);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 10;

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
        [HttpPost]
        public JsonResult Create([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_poster,movie_actor,movie_director,movie_status,rate,movie_banner")] movy movy)
        {
            if (ModelState.IsValid)
            {
                db.movies.Add(movy);
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm phim thành công" });
            }
            return Json(new { success = true, message = "Thêm phim thành công" });
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
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" });
            }
            return Json(new { success = false, message = "Xóa thành công" });
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
