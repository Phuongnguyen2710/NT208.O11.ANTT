using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TicketLand_project.Models;

namespace TicketLand_project.WebAPI
{
    public class moviesController : ApiController
    {
        private QUANLYXEMPHIMEntities db = new QUANLYXEMPHIMEntities();

        // GET: api/movies
        public IQueryable<movy> Getmovies()
        {
            return db.movies;
        }

        // GET: api/movies/5
        [ResponseType(typeof(movy))]
        public async Task<IHttpActionResult> Getmovy(int id)
        {
            movy movy = await db.movies.FindAsync(id);
            if (movy == null)
            {
                return NotFound();
            }

            return Ok(movy);
        }

        // PUT: api/movies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putmovy(int id, movy movy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movy.movie_id)
            {
                return BadRequest();
            }

            db.Entry(movy).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!movyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/movies
        [ResponseType(typeof(movy))]
        public async Task<IHttpActionResult> Postmovy(movy movy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.movies.Add(movy);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = movy.movie_id }, movy);
        }

        // DELETE: api/movies/5
        [ResponseType(typeof(movy))]
        public async Task<IHttpActionResult> Deletemovy(int id)
        {
            movy movy = await db.movies.FindAsync(id);
            if (movy == null)
            {
                return NotFound();
            }

            db.movies.Remove(movy);
            await db.SaveChangesAsync();

            return Ok(movy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool movyExists(int id)
        {
            return db.movies.Count(e => e.movie_id == id) > 0;
        }
    }
}