using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicketLand_project.Models;

namespace TicketLand_project.Controllers
{
    public class moviesController : Controller
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: movies
        public ActionResult Index()
        {
            return View(db.movies.ToList());
        }

        public JsonResult GetData()
        {
            var data = db.movies.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // GET: movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: movies/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_poster,movie_actor,movie_director,movie_status,rate")] movy movy)
        {
            if (ModelState.IsValid)
            {
                db.movies.Add(movy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movy);
        }

        // GET: movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "movie_id,movie_name,movie_description,movie_trailer,movie_cens,movie_genres,movie_release,movie_duration,movie_format,movie_poster,movie_actor,movie_director,movie_status,rate")] movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movy);
        }

        // GET: movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            movy movy = db.movies.Find(id);
            db.movies.Remove(movy);
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
