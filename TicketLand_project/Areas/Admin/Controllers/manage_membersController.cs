using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class manage_membersController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/manage_members
        public ActionResult Index()
        {
            var members = db.members.Include(m => m.ROLE);
            return View(members.ToList());
        }

        // GET: Admin/manage_members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        //Create string MD5
        public static string GetMD5(string str)
        {
            string byte2String = null;
            if (str != null)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);

                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String += targetData[i].ToString("x2");

                }
            }
            return byte2String;
        }

        // GET: Admin/manage_members/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.ROLEs, "role_id", "role_name");
            return View();
        }

        // POST: Admin/manage_members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "member_id,member_name,username,password,gender,date_of_birth,email,city,phone,role_id,member_avatar,member_point")] member member)
        {
            if (ModelState.IsValid)
            {
                var check_username = db.members.Any(x => x.username.Equals(member.username));

                if (check_username)
                {
                    ModelState.AddModelError("username", "Tên đăng nhập đã tồn tại");
                }
                else if (!check_username)
                {
                    //Hash mật khẩu -> lưu
                    member.password = GetMD5(member.password);
                    db.members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.role_id = new SelectList(db.ROLEs, "role_id", "role_name", member.role_id);
            return View(member);
        }

        // GET: Admin/manage_members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.ROLEs, "role_id", "role_name", member.role_id);
            return View(member);
        }

        // POST: Admin/manage_members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "member_id,member_name,username,password,gender,date_of_birth,email,city,phone,role_id,member_point")] member member, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;

                // Xử lý hình ảnh
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(imageFile.InputStream))
                    {
                        byte[] imageData = binaryReader.ReadBytes(imageFile.ContentLength);
                        member.member_avatar = Convert.ToBase64String(imageData);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.ROLEs, "role_id", "role_name", member.role_id);
            return View(member);
        }

        // GET: Admin/manage_members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Admin/manage_members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            member member = db.members.Find(id);
            db.members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //// POST: Admin/manage_members/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public JsonResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        member member = db.members.Find(id);
        //        if (member == null)
        //        {
        //            return Json(new { success = false, errorMessage = "Member not found" });
        //        }

        //        db.members.Remove(member);
        //        db.SaveChanges();

        //        // Trả về kết quả JSON
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Ghi log ngoại lệ
        //        Console.WriteLine($"Exception: {ex.Message}");
        //        return Json(new { success = false, errorMessage = ex.Message });
        //    }
        //}


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
