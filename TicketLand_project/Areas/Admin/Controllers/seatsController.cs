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

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class seatsController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/seats
        public ActionResult Index()
        {
            var seats = db.seats.Include(s => s.room);
            return View(seats.ToList());
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
            return View();
        }

        //// POST: Admin/seats/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "seat_id,seat_type,room_id,row,number,seats_status")] seat seat)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.seats.Add(seat);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", seat.room_id);
        //    return View(seat);
        //}


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
                        seats_status = selectedSeat.seats_status,
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "seat_id,seat_type,room_id,row,number,seats_status")] seat seat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", seat.room_id);
            return View(seat);
        }

        // GET: Admin/seats/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            seat seat = db.seats.Find(id);
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
