﻿using System;
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
        readonly IPdfService _pdfService;
        readonly ISvgService _svgService;
        private ApplicationDbContext db = new ApplicationDbContext();


        public ScalableVectorGraphicsController(IPdfService pdfService, ISvgService svgService)
        {
            _pdfService = pdfService;
            _svgService = svgService;
        }

        // GET: ScalableVectorGraphics
        public async Task<ActionResult> Index(string message)
        {
          var sgvFullList = new List<SgvWithResizedInline>();
          var sgvList =  await db.ScalableVectorGraphics.ToListAsync();

            foreach (var sgv in sgvList)
            {
                var sgvFullItem = new SgvWithResizedInline();
                sgvFullItem.sgv = sgv;
                sgvFullItem.SgvResized = sgv.SgvSpecs.Length <= 200 ? sgv.SgvSpecs : (sgv.SgvSpecs.Substring(0, 200) + "...");      
                sgvFullItem.sgv.SgvSpecs = _svgService.SgvResize(sgv.SgvSpecs,200,200);

                sgvFullList.Add(sgvFullItem);

            }
            if (!string.IsNullOrWhiteSpace(message)) ViewBag.Message = message;
           

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
            sgvWithResized.SgvResized = _svgService.SgvResize(scalableVectorGraphic.SgvSpecs,300,300);

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
              
            if (image != null)
            {               
                scalableVectorGraphic.SgvSpecs = _svgService.getXmlfromFile(image);
            }

            if (_svgService.ValidateInlineSgv(scalableVectorGraphic.SgvSpecs))
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
            string _message;
            var listOfSgv =  db.ScalableVectorGraphics.ToList();

            if (listOfSgv.Any())
            {
                try
                {
                    var stream = _pdfService.MultipleItems(listOfSgv);
                    return File(stream, "application/pdf", "svgList.pdf");
                }
                catch (Exception ex)
                {
                    _message = ex.Message;              
                }
            }
            else
            {

                _message = "PDF can not be generated bacause your table contains no items";
            }

            return RedirectToAction("Index", "ScalableVectorGraphics", new { message = _message });

        }

        public ActionResult SeedDatabase()
        {
            RestSeeder restSeeder = new RestSeeder();
            restSeeder.SeedSgvItems();

            return RedirectToAction("Index") ;

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
        [ValidateAntiForgeryToken]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,SgvSpecs,DateCreated")] ScalableVectorGraphic scalableVectorGraphic, HttpPostedFileBase image)
        {

            if (image != null)
            {
               
                scalableVectorGraphic.SgvSpecs = _svgService.getXmlfromFile(image);
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
