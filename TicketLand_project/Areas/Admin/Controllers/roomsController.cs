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
    public class roomsController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/rooms
        public ActionResult Index()
        {
            return View(db.rooms.ToList());
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

        // GET: Admin/rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "room_id,room_name,capacity")] room room)
        {
            if (ModelState.IsValid)
            {
                db.rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "room_id,room_name,capacity")] room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
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


        public ActionResult GetModalContent(int roomId)
        {
            // Lấy dữ liệu phòng từ cơ sở dữ liệu hoặc từ nơi nào đó
            var room = db.rooms.Find(roomId);// Lấy thông tin phòng dựa trên roomId;

            // Trả về PartialView của modal với dữ liệu của phòng
            return PartialView("DeletePartialView", room);
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
