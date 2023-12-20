using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class roomsController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/rooms
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
            var _rooms = (from l in db.rooms
                           select l).OrderBy(x => x.room_id);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 10;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các member được phân trang theo kích thước và số trang.
            return View(_rooms.ToPagedList(pageNumber, pageSize));

        }

        // GET: Admin/rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //// GET: Admin/rooms/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "room_id,room_name,capacity")] room room)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Kiểm tra định dạng của room_name (ví dụ: Phòng 3)
        //        if (IsValidRoomNameFormat(room.room_name))
        //        {
        //            db.rooms.Add(room);
        //            db.SaveChanges();
        //            return View();
        //        }
        //    }
        //}

        //// POST: Admin/rooms/Create
        //[HttpPost]
        //public JsonResult Create([Bind(Include = "room_id,room_name,capacity")] room room)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Kiểm tra định dạng của room_name (ví dụ: Phòng 3)
        //        if (IsValidRoomNameFormat(room.room_name))
        //        {
        //            db.rooms.Add(room);
        //            db.SaveChanges();
        //            return Json(new { success = true, message = "Lưu thành công" });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Định dạng room_name không hợp lệ" });
        //        }
        //    }

        //    return Json(new { success = false, message = "Lưu không thành công" });
        //}

        //Kiểm tra nhập phòng
        private bool IsValidRoomNameFormat(string roomName)
        {
            // Sử dụng biểu thức chính quy để kiểm tra định dạng
            string pattern = @"^Phòng \d+$";
            return Regex.IsMatch(roomName, pattern);
        }
        
        public PartialViewResult GetRooms(int? page)
        {
            int pageSize = 10; // Set your desired page size
            int pageNumber = page ?? 1;

            var rooms = db.rooms.ToList().ToPagedList(pageNumber, pageSize);

            return PartialView("Room_partial", rooms);
        }


        // GET: Admin/rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Admin/rooms/Edit/5
        [HttpPost]
        public JsonResult Edit([Bind(Include = "room_id,room_name,capacity")] room room)
        {
            if (ModelState.IsValid)
            {
                if (room.capacity == null)
                {
                    room.capacity = 0;
                }
                if (room.capacity > 50)
                {
                    return Json(new { success = false, message = "Đã quá số lượng ghế cho phép" });
                }
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Chỉnh sửa thành công" });

            }
            return Json(new { success = false, message = "Chỉnh sửa thất bại" });

        }

        // GET: Admin/rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Admin/rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            room room = db.rooms.Find(id);
            db.rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Admin/seats/Create
        public ActionResult Create_Seats()
        {
            return View();
        }


        //Hàm post dữ liệu ghế bằng ajax
        [HttpPost]
        public JsonResult SaveSeats(List<seat> selectedSeats)
        {
            try
            {
                foreach (var selectedSeat in selectedSeats)
                {
                    seat newSeat = new seat
                    {
                        room_id = selectedSeat.room_id,
                        seats_status = true,
                        row = selectedSeat.row,
                        number = selectedSeat.number
                    };

                    if ((newSeat.row == "A" && (newSeat.number == 1 || newSeat.number == 2 || newSeat.number == 9 || newSeat.number == 10)) ||
                        (newSeat.row == "B" && (newSeat.number == 1 || newSeat.number == 2 || newSeat.number == 9 || newSeat.number == 10)) ||
                        (newSeat.row == "C" && (newSeat.number == 1 || newSeat.number == 2 || newSeat.number == 9 || newSeat.number == 10)) ||
                        (newSeat.row == "D" && (newSeat.number == 1 || newSeat.number == 2 || newSeat.number == 9 || newSeat.number == 10)) ||
                        (newSeat.row == "E" && (newSeat.number == 1 || newSeat.number == 2 || newSeat.number == 9 || newSeat.number == 10)))
                    {
                        newSeat.seat_type = "Couple";
                    }
                    else
                    {
                        newSeat.seat_type = "Đơn";
                    }


                    // Thêm mới ghế vào cơ sở dữ liệu
                    db.seats.Add(newSeat);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return Json(new { success = true, message = "Seats saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //Hàm post dữ liệu ghế ngồi ajax
        [HttpGet]
        public JsonResult LoadSeatsData_()
        {
            try
            {
                var _seats = db.seats.Select(s => new
                {
                    seatId = s.seat_id,
                    seatNumber = s.number,
                    seatRow = s.row,
                    seatType = s.seat_type,
                    Room = new
                    {
                        RoomId = s.room.room_id,
                    }
                }).ToList();

                return Json(new { seats = _seats }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
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
