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
    public class schedulesController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/schedules
        public ActionResult Index(int? page)
        {
            var username = Session["Username"] as string;
            var idMember = Session["idMember"] as string;
            int Idmember;

            // Convert sang int
            int.TryParse(idMember, out Idmember);

            var member = db.members.SingleOrDefault(m => m.member_id == Idmember);

            if (member == null || member.role_id == 2)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            // 1. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo memberID mới có thể phân trang.
            var schedulesQuery = (from l in db.schedules
                                  select l).OrderBy(x => x.movie_id);

            // 2. Lấy dữ liệu từ truy vấn
            var schedulesList = schedulesQuery.ToList();

            // 3. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 100;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 4. Phân trang trên danh sách đã lấy
            var pagedList = schedulesList.ToPagedList(pageNumber, pageSize);

            // 5. Tạo SelectList từ danh sách đã lấy
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name");
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name");

            // 6. Trả về view với danh sách đã phân trang
            return View(pagedList);
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
        public JsonResult Create([Bind(Include = "schedule_id,movie_id,room_id,time_start,time_end,show_date")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedules.Add(schedule);
                db.SaveChanges();
                return Json(new { success = true, message = "Thêm thành công" });
            }
            return Json(new { success = false, message = "Thêm thất bại!" });
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


        // POST: Admin/schedules/Edit/5
        [HttpPost]
        public JsonResult Edit([Bind(Include = "schedule_id,movie_id,room_id,time_start,show_date")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                    return Json(new { success = true, message = "Thay đổi thành công" });
                
            }

            // Load danh sách phim và danh sách phòng để hiển thị lại trong trường hợp lỗi
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_name", schedule.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_name", schedule.room_id);

            return Json(new { success = true, message = "Thay đổi thất bại" });
        }



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
