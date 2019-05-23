using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Products.Models;

namespace JobsV1.Areas.Products.Controllers
{
    public class SmRateUoMsController : Controller
    {
        private ProdDBContainer db = new ProdDBContainer();

        // GET: Products/SmRateUoMs
        public ActionResult Index()
        {
            return View(db.SmRateUoMs.ToList());
        }

        // GET: Products/SmRateUoMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmRateUoM smRateUoM = db.SmRateUoMs.Find(id);
            if (smRateUoM == null)
            {
                return HttpNotFound();
            }
            return View(smRateUoM);
        }

        // GET: Products/SmRateUoMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/SmRateUoMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] SmRateUoM smRateUoM)
        {
            if (ModelState.IsValid)
            {
                db.SmRateUoMs.Add(smRateUoM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smRateUoM);
        }

        // GET: Products/SmRateUoMs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmRateUoM smRateUoM = db.SmRateUoMs.Find(id);
            if (smRateUoM == null)
            {
                return HttpNotFound();
            }
            return View(smRateUoM);
        }

        // POST: Products/SmRateUoMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] SmRateUoM smRateUoM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smRateUoM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smRateUoM);
        }

        // GET: Products/SmRateUoMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmRateUoM smRateUoM = db.SmRateUoMs.Find(id);
            if (smRateUoM == null)
            {
                return HttpNotFound();
            }
            return View(smRateUoM);
        }

        // POST: Products/SmRateUoMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmRateUoM smRateUoM = db.SmRateUoMs.Find(id);
            db.SmRateUoMs.Remove(smRateUoM);
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
