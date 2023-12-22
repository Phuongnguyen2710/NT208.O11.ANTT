using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TicketLand_project.Models;
using PagedList;
using System.Web.UI;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class seatsController : Controller
    {

        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/seats
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

            if (page == null)
                page = 1;

            int pageSize = 11;
            int pageNumber = (page ?? 1);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var seatsData = db.seats.ToList();

            // Sắp xếp dữ liệu sử dụng hàm GetRowOrder đã tính toán
            var links = seatsData
                .OrderBy(x => x.room_id)
                .ThenBy(x => GetRowOrder(x.row))
                .ThenBy(x => x.number)
                .ToPagedList(pageNumber, pageSize);

            return View(links);
        }

        int GetRowOrder(string row)
        {
            // Chuyển đổi chữ cái thành số bằng cách sử dụng mã ASCII
            int rowNumber = (int)row.ToUpper()[0] - (int)'A' + 1;

            return rowNumber;
        }


        // GET: Admin/seats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seat seat = db.seats.Find(id);
            if (seat == null)
            {
                return HttpNotFound();
            }
            return View(seat);
        }

        // GET: Admin/seats/Create
        public ActionResult Create()
        {
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name");
            ViewBag.room_capacity = new SelectList(db.rooms, "room_id", "capacity");
            return View();
        }


        //// POST: seats/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult Create([Bind(Include = "seat_id,seat_type,room_id,row,number,seats_status")] seat seat)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.seats.Add(seat);
        //        db.SaveChanges();
        //        return Json(new { success = true, message = "Lưu thành công" });
        //    }

        //    ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", seat.room_id);
        //    return Json(new { success = false, message = "Lưu không thành công" });
        //}

        [HttpGet]
        public JsonResult GetRoomCapacity(int roomId)
        {
            // Lấy capacity từ cơ sở dữ liệu dựa trên roomId
            var capacity = db.rooms.Find(roomId)?.capacity;

            // Trả về capacity dưới dạng JSON
            return Json(capacity, JsonRequestBehavior.AllowGet);
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

                    // Check trùng nhau để báo lỗi
                    var check_unique = db.seats.Any(x => x.row == newSeat.row && x.number == newSeat.number && x.room_id == newSeat.room_id);
                    if (check_unique)
                    {
                        return Json(new { success = false, message = "Ghế đã được set up!" });
                    }

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


        // GET: Admin/seats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seat seat = db.seats.Find(id);
            if (seat == null)
            {
                return HttpNotFound();
            }
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", seat.room_id);
            return View(seat);
        }

        // POST: Admin/seats/Edit/5
        [HttpPost]
        public JsonResult Edit([Bind(Include = "seat_id,seat_type,room_id,row,number,seats_status")] seat seat)
        {

            if (ModelState.IsValid)
            {

                db.Entry(seat).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Thay đổi thành công" });
            }
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", seat.room_id);
            return Json(new { success = false, message = "Chỉnh sửa thất bại" });
        }


        // POST: Admin/seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            seat seat = db.seats.Find(id);
            if (seat == null)
            {
                return HttpNotFound();
            }
            db.seats.Remove(seat);
            db.SaveChanges();
            return RedirectToAction("Index");
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
