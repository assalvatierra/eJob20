using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogDriversController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogDrivers
        public ActionResult Index()
        {
            return View(db.crLogDrivers.OrderBy(c=> c.OrderNo ?? 999).ToList());
        }

        // GET: Personel/crLogDrivers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Create
        public ActionResult Create()
        {
            crLogDriver crLogDriver = new crLogDriver();
            crLogDriver.OrderNo = 100;

            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, Contact1, Contact2, OrderNo")] crLogDriver crLogDriver)
        {
            if (ModelState.IsValid && InputValidation(crLogDriver))
            {
                db.crLogDrivers.Add(crLogDriver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1, Contact2, OrderNo")] crLogDriver crLogDriver)
        {
            if (ModelState.IsValid && InputValidation(crLogDriver))
            {
                db.Entry(crLogDriver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            db.crLogDrivers.Remove(crLogDriver);
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

        public bool InputValidation(crLogDriver crLogDriver)
        {
            bool isValid = true;

            if (crLogDriver.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }


            return isValid;
        }
    }
}
