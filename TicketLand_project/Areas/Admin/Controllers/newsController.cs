using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class newsController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/news
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
            var _news = (from l in db.news
                           select l).OrderBy(x => x.news_release);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 11;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các member được phân trang theo kích thước và số trang.
            return View(_news.ToPagedList(pageNumber, pageSize));

        }

        // GET: Admin/news/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/news/Create
        public ActionResult Create()
        {
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name");
            return View();
        }

        public static string GenerateSlug(string title)
        {
            string slug = Regex.Replace(title, @"à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ", "a");
            slug = Regex.Replace(slug, @"è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ", "e");
            slug = Regex.Replace(slug, @"ì|í|ị|ỉ|ĩ", "i");
            slug = Regex.Replace(slug, @"ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ", "o");
            slug = Regex.Replace(slug, @"ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ", "u");
            slug = Regex.Replace(slug, @"ỳ|ý|ỵ|ỷ|ỹ", "y");
            slug = Regex.Replace(slug, @"đ", "d");

            return slug.ToLowerInvariant().Replace(" ", "-");
        }

        // POST: Admin/news/Create
        //Xử lý Tạo mới có ajax
        [HttpPost] 
        public JsonResult Create([Bind(Include = "news_id,movie_id,news_title,news_content,news_release")] news news, HttpPostedFileBase news_img)
        {
            if (ModelState.IsValid)
            {
                if (news_img != null && news_img.ContentLength > 0)
                {
                    var slug = GenerateSlug(news.news_title);
                    var posterFileName = Path.GetFileName(news_img.FileName);
                    var extension = Path.GetExtension(posterFileName);
                    var new_file_name = $"{slug}{extension}";
                    var relativePosterPath = "\\Assets\\img\\home\\news\\" + new_file_name;
                    var absolutePosterPath = Path.Combine(Server.MapPath("~/Assets/img/home/news/"), new_file_name);

                    news.news_img = relativePosterPath;

                    // Lưu file vào máy chủ với tên mới
                    using (var fileStream = new FileStream(absolutePosterPath, FileMode.Create))
                    {
                        news_img.InputStream.Seek(0, SeekOrigin.Begin);
                        news_img.InputStream.CopyTo(fileStream);
                    }
                }

                db.news.Add(news);
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm thành công" });
            }

            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", news.movie_id);
            return Json(new { success = true, message = "Thêm thất bại!" }); ;
        }
        //Kết thúc 


        // GET: Admin/news/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", news.movie_id);
            return View(news);
        }

        // POST: Admin/news/Edit/5
        [HttpPost]

        public ActionResult Edit([Bind(Include = "news_id,movie_id,news_title,news_content,news_img,news_release")] news news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                // Chuyển hướng về trang Index
                return RedirectToAction("Index");
            }

            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", news.movie_id);
            // Truyền thông điệp lỗi vào ViewBag và trả về View
            ViewBag.ErrorMessage = "Chỉnh sửa thất bại!";
            return View(); // Tùy thuộc vào tên của view bạn muốn sử dụng
        }

        // GET: Admin/news/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/news/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            news news = db.news.Find(id);
            if (news != null)
            {
                db.news.Remove(news);
                db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công" });
            }
            return Json(new { success = false, message = "Xóa thất bại!" });
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
