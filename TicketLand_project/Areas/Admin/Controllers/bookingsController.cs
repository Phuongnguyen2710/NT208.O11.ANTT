using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class bookingsController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/bookings
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

            ViewBag.member_id = new SelectList(db.members, "member_id", "member_name");
            ViewBag.schedule_id = new SelectList(db.schedules, "schedule_id", "show_date");
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name");
            ViewBag.check_movie_shl = new SelectList(db.schedules, "schedule_id", "movie_id");
            ViewBag.seat_id = new SelectList(db.seats, "seat_id", "seat_type");
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name");

            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo memberID mới có thể phân trang.
            // Sử dụng phương thức LINQ
            var bookings = db.bookings
                .Include(b => b.member)
                .Include(b => b.schedule)
                .Include(b => b.booking_detail)
                .OrderBy(x => x.booking_id)
                .ToList();

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 11;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các member được phân trang theo kích thước và số trang.
            return View(bookings.ToPagedList(pageNumber, pageSize));
        }


        public JsonResult GetSchedulesForMovie(int movieId)
        {
            // Lấy danh sách các lịch chiếu dựa trên id của phim
            var schedules = db.schedules
                .Where(s => s.movie_id == movieId)
                .Select(s => new
                {
                    value = s.schedule_id,
                    text = s.show_date.ToString() 
                })
                .ToList();

            // Trả về dữ liệu dưới dạng JSON
            return Json(schedules, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Booking_Detail(int? booking_id)
        {
            if (booking_id == null)
            {
                // Xử lý khi booking_id không được truyền
                return RedirectToAction("Index"); // Ví dụ chuyển hướng đến trang Index
            }

            var bookingDetails = db.booking_detail
                                  .Where(bd => bd.booking_id == booking_id)
                                  .ToList();
            ViewBag.booking_id = booking_id;
            return View(bookingDetails);
        }



        // GET: Admin/bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Admin/bookings/Create
        public ActionResult Create()
        {
            ViewBag.member_id = new SelectList(db.members, "member_id", "member_name");
            ViewBag.schedule_id = new SelectList(db.schedules, "schedule_id", "schedule_id");
            return View();
        }

        // POST: Admin/bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "booking_id,member_id,schedule_id,booking_status,booking_date,total_price")] booking booking)
        {
            if (ModelState.IsValid)
            {
                db.bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.member_id = new SelectList(db.members, "member_id", "member_name", booking.member_id);
            ViewBag.schedule_id = new SelectList(db.schedules, "schedule_id", "schedule_id", booking.schedule_id);
            return View(booking);
        }

        // GET: Admin/bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.member_id = new SelectList(db.members, "member_id", "member_name", booking.member_id);
            ViewBag.schedule_id = new SelectList(db.schedules, "schedule_id", "schedule_id", booking.schedule_id);
            return View(booking);
        }

        // POST: Admin/bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "booking_id,member_id,schedule_id,booking_status,booking_date,total_price")] booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.member_id = new SelectList(db.members, "member_id", "member_name", booking.member_id);
            ViewBag.schedule_id = new SelectList(db.schedules, "schedule_id", "schedule_id", booking.schedule_id);
            return View(booking);
        }

        // GET: Admin/bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking booking = db.bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        //// POST: Admin/bookings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    booking booking = db.bookings.Find(id);
        //    db.bookings.Remove(booking);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // POST: Admin/bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            try
            {
                booking booking = db.bookings.Find(id);

                if (booking == null)
                {
                    // Trả về một đối tượng JSON thông báo lỗi nếu không tìm thấy booking
                    return Json(new { success = false, message = "Không tìm thấy booking để xóa." });
                }

                db.bookings.Remove(booking);
                db.SaveChanges();

                // Trả về một đối tượng JSON thông báo thành công
                return Json(new { success = true, message = "Xóa thành công." });
            }
            catch (Exception ex)
            {
                // Trả về một đối tượng JSON thông báo lỗi nếu có lỗi xóa
                return Json(new { success = false, message = $"Lỗi khi xóa: {ex.Message}" });
            }
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
