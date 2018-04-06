using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SgvToPdf.Models;
using Svg;
using System.Xml;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SgvToPdf.Services;
using SgvToPdf.ViewModels;

namespace SgvToPdf.Controllers
{
    public class ScalableVectorGraphicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScalableVectorGraphics
        public async Task<ActionResult> Index()
        {
          var sgvFullList = new List<SgvWithResizedInline>();
          var sgvList =  await db.ScalableVectorGraphics.ToListAsync();

            foreach (var sgv in sgvList)
            {
                var sgvFullItem = new SgvWithResizedInline();
                sgvFullItem.sgv = sgv;
                sgvFullItem.SgvResized = sgv.SgvSpecs.Length <= 200 ? sgv.SgvSpecs : (sgv.SgvSpecs.Substring(0, 200) + "...");

                ISgvService sgvResized = new SgvNetService();
                sgvFullItem.sgv.SgvSpecs = sgvResized.SgvResize(sgv.SgvSpecs,200,200);

                sgvFullList.Add(sgvFullItem);

            }
           

            return View(sgvFullList);
        }

        // GET: ScalableVectorGraphics/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalableVectorGraphic scalableVectorGraphic = await db.ScalableVectorGraphics.FindAsync(id);
            if (scalableVectorGraphic == null)
            {
                return HttpNotFound();
            }

            var sgvWithResized = new SgvWithResizedInline();
            sgvWithResized.sgv = scalableVectorGraphic;
            

            ISgvService sgvResized = new SgvNetService();
            sgvWithResized.SgvResized = sgvResized.SgvResize(scalableVectorGraphic.SgvSpecs,300,300);

            return View(sgvWithResized);
        }

        // GET: ScalableVectorGraphics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScalableVectorGraphics/Create
        [HttpPost, ValidateInput(false)]//TODO: Find a better Solution
        [ValidateAntiForgeryToken]     
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,SgvSpecs")] ScalableVectorGraphic scalableVectorGraphic, HttpPostedFileBase image)
        {
            ISgvService sgvService = new SgvNetService();          
            if (image != null)
            {               
                scalableVectorGraphic.SgvSpecs = sgvService.getXmlfromFile(image);
            }

            if (sgvService.ValidateInlineSgv(scalableVectorGraphic.SgvSpecs))
            {
                scalableVectorGraphic.DateCreated = DateTime.UtcNow;
                if (ModelState.IsValid)
                {
                    scalableVectorGraphic.Id = Guid.NewGuid();
                    db.ScalableVectorGraphics.Add(scalableVectorGraphic);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
   
                ModelState.AddModelError("","The information for the sgv specification could be wrong, please check your file");

            }

            return View(scalableVectorGraphic);
        }
        public ActionResult GeneratePdf()
        {

            var listOfSgv =  db.ScalableVectorGraphics.ToList();

            IPdfService pdfWriter = new PdfServiceItextSharp();
            var stream = pdfWriter.MultipleItems(listOfSgv);

            return File(stream, "application/pdf", "DownloadName.pdf");

        }




        // GET: ScalableVectorGraphics/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalableVectorGraphic scalableVectorGraphic = await db.ScalableVectorGraphics.FindAsync(id);
            if (scalableVectorGraphic == null)
            {
                return HttpNotFound();
            }
            return View(scalableVectorGraphic);
        }

        // POST: ScalableVectorGraphics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,SgvSpecs,DateCreated")] ScalableVectorGraphic scalableVectorGraphic, HttpPostedFileBase image)
        {

            if (image != null)
            {
                ISgvService sgvToBitmap = new SgvNetService();
                scalableVectorGraphic.SgvSpecs = sgvToBitmap.getXmlfromFile(image);
            }

            if (ModelState.IsValid)
            {
                db.Entry(scalableVectorGraphic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(scalableVectorGraphic);
        }

        // GET: ScalableVectorGraphics/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScalableVectorGraphic scalableVectorGraphic = await db.ScalableVectorGraphics.FindAsync(id);
            if (scalableVectorGraphic == null)
            {
                return HttpNotFound();
            }
            return View(scalableVectorGraphic);
        }

        // POST: ScalableVectorGraphics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ScalableVectorGraphic scalableVectorGraphic = await db.ScalableVectorGraphics.FindAsync(id);
            db.ScalableVectorGraphics.Remove(scalableVectorGraphic);
            await db.SaveChangesAsync();
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
