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
    public class crLogOwnersController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogOwners
        public ActionResult Index()
        {
            return View(db.crLogOwners.ToList());
        }

        // GET: Personel/crLogOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOwner crLogOwner = db.crLogOwners.Find(id);
            if (crLogOwner == null)
            {
                return HttpNotFound();
            }
            return View(crLogOwner);
        }

        // GET: Personel/crLogOwners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personel/crLogOwners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Mobile,Remarks")] crLogOwner crLogOwner)
        {
            if (ModelState.IsValid)
            {
                db.crLogOwners.Add(crLogOwner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crLogOwner);
        }

        // GET: Personel/crLogOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOwner crLogOwner = db.crLogOwners.Find(id);
            if (crLogOwner == null)
            {
                return HttpNotFound();
            }
            return View(crLogOwner);
        }

        // POST: Personel/crLogOwners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Mobile,Remarks")] crLogOwner crLogOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crLogOwner);
        }

        // GET: Personel/crLogOwners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogOwner crLogOwner = db.crLogOwners.Find(id);
            if (crLogOwner == null)
            {
                return HttpNotFound();
            }
            return View(crLogOwner);
        }

        // POST: Personel/crLogOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogOwner crLogOwner = db.crLogOwners.Find(id);
            db.crLogOwners.Remove(crLogOwner);
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
