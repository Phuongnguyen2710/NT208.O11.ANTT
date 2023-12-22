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
    public class schedulesController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/schedules
        public ActionResult Index()
        {
            var schedules = db.schedules.Include(s => s.movy).Include(s => s.room);
            return View(schedules.ToList());
        }

        //// GET: Admin/schedules/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    schedule schedule = db.schedules.Find(id);
        //    if (schedule == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(schedule);
        //}

        // GET: Admin/schedules/Create
        public ActionResult Create()
        {
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name");
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name");
            return View();
        }

        // POST: Admin/schedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "schedule_id,movie_id,room_id,time_start,time_end,show_date")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", schedule.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", schedule.room_id);
            return View(schedule);
        }

        // GET: Admin/schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", schedule.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", schedule.room_id);
            return View(schedule);
        }


        //// POST: Admin/schedules/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "schedule_id,movie_id,room_id,time_start,time_end,show_date")] schedule schedule)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Lấy suất chiếu từ cơ sở dữ liệu
        //        var existingSchedule = db.schedules.Find(schedule.schedule_id);

        //        // Kiểm tra xem suất chiếu thuộc cùng phim không
        //        if (existingSchedule != null && existingSchedule.movie_id == schedule.movie_id)
        //        {
        //            // Cập nhật thông tin suất chiếu
        //            existingSchedule.room_id = schedule.room_id;
        //            existingSchedule.time_start = schedule.time_start;
        //            existingSchedule.time_end = schedule.time_end;
        //            existingSchedule.show_date = schedule.show_date;

        //            // Lưu thay đổi vào cơ sở dữ liệu
        //            db.SaveChanges();

        //            return RedirectToAction("Index");
        //        }
        //    }

        //    // Load danh sách phim và danh sách phòng để hiển thị lại trong trường hợp lỗi
        //    ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", schedule.movie_id);
        //    ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", schedule.room_id);

        //    return View(schedule);
        //}



        // GET: Admin/schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Admin/schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            schedule schedule = db.schedules.Find(id);
            db.schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Trả về Id phòng
        private int GetRoomNumber(string roomName)
        {
            // Giả sử tên phòng có định dạng "Phòng X", với X là số phòng
            string numberPart = roomName.Substring(6); // Bỏ qua "Phòng "
            int roomNumber = int.Parse(numberPart);
            return roomNumber;
        }


        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schedules = db.schedules
                .Where(s => s.movy.movie_id == id)
                .ToList()
                .OrderBy(s => GetRoomNumber(s.room.room_name))
                .ToList();
            return View(schedules);
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
