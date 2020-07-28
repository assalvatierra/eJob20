using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogOdoesController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();

        // GET: Personel/crLogOdoes
        public ActionResult Index()
        {
            var crLogOdoes = db.crLogOdoes.Include(c => c.crLogUnit).Include(c => c.crLogDriver);
            return View(crLogOdoes.ToList());
        }

        // GET: Personel/crLogOdoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOdo crLogOdo = db.crLogOdoes.Find(id);
            if (crLogOdo == null)
            {
                return HttpNotFound();
            }
            return View(crLogOdo);
        }

        // GET: Personel/crLogOdoes/Create
        public ActionResult Create()
        {
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description");
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            return View();
        }

        // POST: Personel/crLogOdoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crLogUnitId,crLogDriverId,OdoCurrent,dtReading,Remarks")] crLogOdo crLogOdo)
        {
            if (ModelState.IsValid)
            {
                db.crLogOdoes.Add(crLogOdo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogOdo.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogOdo.crLogDriverId);
            return View(crLogOdo);
        }

        // GET: Personel/crLogOdoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOdo crLogOdo = db.crLogOdoes.Find(id);
            if (crLogOdo == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogOdo.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogOdo.crLogDriverId);
            return View(crLogOdo);
        }

        // POST: Personel/crLogOdoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,crLogUnitId,crLogDriverId,OdoCurrent,dtReading,Remarks")] crLogOdo crLogOdo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogOdo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogOdo.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogOdo.crLogDriverId);
            return View(crLogOdo);
        }

        // GET: Personel/crLogOdoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOdo crLogOdo = db.crLogOdoes.Find(id);
            if (crLogOdo == null)
            {
                return HttpNotFound();
            }
            return View(crLogOdo);
        }

        // POST: Personel/crLogOdoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogOdo crLogOdo = db.crLogOdoes.Find(id);
            db.crLogOdoes.Remove(crLogOdo);
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
