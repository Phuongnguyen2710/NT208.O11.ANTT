using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class booking_detailController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/booking_detail
        public ActionResult Index(int booking_id)
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
            var booking_detail = db.booking_detail.Include(b => b.booking).Include(b => b.seat);
  
            return View(booking_detail.ToList());
        }

        // GET: Admin/booking_detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking_detail booking_detail = db.booking_detail.Find(id);
            if (booking_detail == null)
            {
                return HttpNotFound();
            }
            return View(booking_detail);
        }



        [HttpGet]
        public ActionResult Booking(int booking_id)
        {
            var booking_detail = db.booking_detail.Include(b => b.booking).Include(b => b.seat);
            var bookingDetails = db.booking_detail
                                    .Where(bd => bd.booking_id == booking_id)
                                    .ToList();
            return View(bookingDetails.ToList());
        }




        // GET: Admin/booking_detail/Create
        public ActionResult Create()
        {
            ViewBag.booking_id = new SelectList(db.bookings, "booking_id", "total_price");
            ViewBag.seat_id = new SelectList(db.seats, "seat_id", "seat_type");
            return View();
        }

        // POST: Admin/booking_detail/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "bkdetail_id,booking_id,seat_id")] booking_detail booking_detail)
        {
            if (ModelState.IsValid)
            {
                db.booking_detail.Add(booking_detail);
                db.SaveChanges();
                return RedirectToAction("Index","bookings");
            }

            ViewBag.booking_id = new SelectList(db.bookings, "booking_id", "total_price", booking_detail.booking_id);
            ViewBag.seat_id = new SelectList(db.seats, "seat_id", "seat_type", booking_detail.seat_id);
            return View(booking_detail);
        }

        // GET: Admin/booking_detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking_detail booking_detail = db.booking_detail.Find(id);
            if (booking_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.booking_id = new SelectList(db.bookings, "booking_id", "total_price", booking_detail.booking_id);
            ViewBag.seat_id = new SelectList(db.seats, "seat_id", "seat_type", booking_detail.seat_id);
            return View(booking_detail);
        }

        // POST: Admin/booking_detail/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string row, int number)
        {
            // Tìm booking_detail cần sửa theo id
            var bookingDetail = db.booking_detail.Find(id);

            if (bookingDetail == null)
            {
                return HttpNotFound();
            }

            // Cập nhật thông tin mới từ form
            bookingDetail.seat.row = row;
            bookingDetail.seat.number = number;

            // Gán seat_type dựa trên điều kiện
            if ((row == "A" && (number == 1 || number == 2 || number == 9 || number == 10)) ||
                (row == "B" && (number == 1 || number == 2 || number == 9 || number == 10)) ||
                (row == "C" && (number == 1 || number == 2 || number == 9 || number == 10)) ||
                (row == "D" && (number == 1 || number == 2 || number == 9 || number == 10)) ||
                (row == "E" && (number == 1 || number == 2 || number == 9 || number == 10)))
            {
                bookingDetail.seat.seat_type = "Couple";
            }
            else
            {
                bookingDetail.seat.seat_type = "Đơn";
            }

            // Kiểm tra trùng lặp
            var isDuplicate = db.booking_detail.Any(bd => bd.seat_id == bookingDetail.seat.seat_id && bd.bkdetail_id != bookingDetail.bkdetail_id);

            if (isDuplicate)
            {
                return Json(new { success = false, message = "Ghế đã được set up!" });
            }

            // Kiểm tra ModelState
            if (ModelState.IsValid)
            {
                try
                {
                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.Entry(bookingDetail).State = EntityState.Modified;
                    db.SaveChanges();

                    // Trả về kết quả JSON nếu thành công
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi và trả về kết quả JSON với thông báo lỗi
                    return Json(new { success = false, error = ex.Message });
                }
            }

            // Nếu có lỗi trong ModelState, trả về kết quả JSON với thông báo lỗi
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors = errors });
        }



        // GET: Admin/booking_detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            booking_detail booking_detail = db.booking_detail.Find(id);
            if (booking_detail == null)
            {
                return HttpNotFound();
            }
            return View(booking_detail);
        }

        // POST: Admin/booking_detail/Delete/5
        [HttpPost, ActionName("Delete")]

        public JsonResult DeleteConfirmed(int id)
        {
            booking_detail booking_detail = db.booking_detail.Find(id);
            if (booking_detail != null)
            {
                db.booking_detail.Remove(booking_detail);
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
