using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using PagedList;
using TicketLand_project.Models;

namespace TicketLand_project.Areas.Admin.Controllers
{
    public class manage_membersController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: Admin/manage_members
        public ActionResult Index(int? page)
        {
            var username = Session["Username"] as string;
            var idMember = Session["idMember"] as string;
            int Idmember;

            // Covert sang int
            int.TryParse(idMember, out Idmember);

            var member = db.members.SingleOrDefault(m => m.member_id == Idmember);

            if (member == null ||member.role_id == 2)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo memberID mới có thể phân trang.
            var _member = (from l in db.members
                         select l).OrderBy(x => x.member_id);

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 9;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            // 5. Trả về các member được phân trang theo kích thước và số trang.
            return View(_member.ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public JsonResult GetUserInfoFromUser()
        {
            string username = Session["Username"] as string;
            string idMember = Session["idMember"] as string;
            string isLogin = Session["IsLoggedIn"] as string;
            string avatar = Session["Avartar"] as string ?? "Null";
            return Json(new { Username = username, IdMember = idMember, IsLogin = isLogin, Avatar = avatar }, JsonRequestBehavior.AllowGet);
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

        // POST: Admin/manage_members/Creates
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

        public static string GenerateSlug(string title)
        {
            string slug = Regex.Replace(title, @"[^a-zA-Z0-9-]", "-");
            slug = Regex.Replace(slug, @"-{2,}", "-");
            slug = slug.Trim('-').ToLower();
            return slug;
        }

        // POST: Admin/manage_members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "member_id,member_name,username,password,gender,date_of_birth,email,city,phone,role_id,member_point")] member member, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;

                //// Xử lý hình ảnh
                //if (imageFile != null && imageFile.ContentLength > 0)
                //{
                //    using (var binaryReader = new BinaryReader(imageFile.InputStream))
                //    {
                //        byte[] imageData = binaryReader.ReadBytes(imageFile.ContentLength);
                //        member.member_avatar = Convert.ToBase64String(imageData);
                //    }
                //}

                // Xử lý trường input kiểu file (avatar)
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var slug = GenerateSlug(member.username);
                    var memberFileName = Path.GetFileName(imageFile.FileName);
                    var extension = Path.GetExtension(memberFileName);
                    var new_file_name = $"{slug}{extension}";
                    var relativePosterPath = "\\Assets\\img\\home\\avatar_member\\" + new_file_name;
                    var absolutePosterPath = Path.Combine(Server.MapPath("~/Assets/img/home/avatar_member/"), new_file_name);

                    member.member_avatar = relativePosterPath;

                    // Lưu file vào máy chủ với tên mới
                    using (var fileStream = new FileStream(absolutePosterPath, FileMode.Create))
                    {
                        imageFile.InputStream.Seek(0, SeekOrigin.Begin);
                        imageFile.InputStream.CopyTo(fileStream);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.ROLEs, "role_id", "role_name", member.role_id);
            return View(member);
        }

 

        // POST: Admin/manage_members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            member member = db.members.Find(id);
            if (member == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
