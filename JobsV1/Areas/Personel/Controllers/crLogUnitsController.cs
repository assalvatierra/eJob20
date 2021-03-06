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
    public class crLogUnitsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" }
                };

        // GET: Personel/crLogUnits
        public ActionResult Index()
        {
            return View(db.crLogUnits.OrderBy(c => c.OrderNo ?? 999).ToList());
        }

        // GET: Personel/crLogUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogUnit crLogUnit = db.crLogUnits.Find(id);
            if (crLogUnit == null)
            {
                return HttpNotFound();
            }
            return View(crLogUnit);
        }

        // GET: Personel/crLogUnits/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.crLogOwnerId = new SelectList(db.crLogOwners, "Id", "Name");
            return View();
        }

        // POST: Personel/crLogUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,OrderNo,Status,crLogOwnerId")] crLogUnit crLogUnit)
        {
            if (ModelState.IsValid && InputValidation(crLogUnit))
            {
                db.crLogUnits.Add(crLogUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogUnit.Status);
            ViewBag.crLogOwnerId = new SelectList(db.crLogOwners, "Id", "Name");
            return View(crLogUnit);
        }

        // GET: Personel/crLogUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogUnit crLogUnit = db.crLogUnits.Find(id);
            if (crLogUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogUnit.Status);
            ViewBag.crLogOwnerId = new SelectList(db.crLogOwners, "Id", "Name", crLogUnit.crLogOwnerId);
            return View(crLogUnit);
        }

        // POST: Personel/crLogUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,OrderNo,Status,crLogOwnerId")] crLogUnit crLogUnit)
        {
            if (ModelState.IsValid && InputValidation(crLogUnit))
            {
                db.Entry(crLogUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogUnit.Status);
            ViewBag.crLogOwnerId = new SelectList(db.crLogOwners, "Id", "Name", crLogUnit.crLogOwnerId);
            return View(crLogUnit);
        }

        // GET: Personel/crLogUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogUnit crLogUnit = db.crLogUnits.Find(id);
            if (crLogUnit == null)
            {
                return HttpNotFound();
            }
            return View(crLogUnit);
        }

        // POST: Personel/crLogUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogUnit crLogUnit = db.crLogUnits.Find(id);
            db.crLogUnits.Remove(crLogUnit);
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


        public bool InputValidation(crLogUnit crLogUnit)
        {
            bool isValid = true;

            if (crLogUnit.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            return isValid;
        }
    }
}
