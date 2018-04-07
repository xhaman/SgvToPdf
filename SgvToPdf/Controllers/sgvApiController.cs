using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SgvToPdf.Models;
using SgvToPdf.Services;

namespace SgvToPdf.Controllers
{
    public class sgvApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/sgvApi
        public IQueryable<ScalableVectorGraphic> GetScalableVectorGraphics()
        {
            return db.ScalableVectorGraphics;
        }

        // GET: api/sgvApi/5
        [ResponseType(typeof(ScalableVectorGraphic))]
        public IHttpActionResult GetScalableVectorGraphic(Guid id)
        {
            ScalableVectorGraphic scalableVectorGraphic = db.ScalableVectorGraphics.Find(id);
            if (scalableVectorGraphic == null)
            {
                return NotFound();
            }

            return Ok(scalableVectorGraphic);
        }

        // PUT: api/sgvApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutScalableVectorGraphic(Guid id, ScalableVectorGraphic scalableVectorGraphic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scalableVectorGraphic.Id)
            {
                return BadRequest();
            }
            ISvgService sgvService = new SgvNetService();
            if (sgvService.ValidateInlineSgv(scalableVectorGraphic.SgvSpecs))
            {


                db.Entry(scalableVectorGraphic).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScalableVectorGraphicExists(id))
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

        // POST: api/sgvApi
        [ResponseType(typeof(ScalableVectorGraphic))]
        public IHttpActionResult PostScalableVectorGraphic(ScalableVectorGraphic scalableVectorGraphic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ISvgService sgvService = new SgvNetService();
            if (sgvService.ValidateInlineSgv(scalableVectorGraphic.SgvSpecs))
            {

                db.ScalableVectorGraphics.Add(scalableVectorGraphic);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ScalableVectorGraphicExists(scalableVectorGraphic.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = scalableVectorGraphic.Id }, scalableVectorGraphic);
        }

        // DELETE: api/sgvApi/5
        [ResponseType(typeof(ScalableVectorGraphic))]
        public IHttpActionResult DeleteScalableVectorGraphic(Guid id)
        {
            ScalableVectorGraphic scalableVectorGraphic = db.ScalableVectorGraphics.Find(id);
            if (scalableVectorGraphic == null)
            {
                return NotFound();
            }

            db.ScalableVectorGraphics.Remove(scalableVectorGraphic);
            db.SaveChanges();

            return Ok(scalableVectorGraphic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ScalableVectorGraphicExists(Guid id)
        {
            return db.ScalableVectorGraphics.Count(e => e.Id == id) > 0;
        }
    }
}