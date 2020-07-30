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
    public class crLogCompaniesController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" }
                };

        // GET: Personel/crLogCompanies
        public ActionResult Index()
        {
            return View(db.crLogCompanies.ToList());
        }

        // GET: Personel/crLogCompanies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCompany crLogCompany = db.crLogCompanies.Find(id);
            if (crLogCompany == null)
            {
                return HttpNotFound();
            }
            return View(crLogCompany);
        }

        // GET: Personel/crLogCompanies/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            return View();
        }

        // POST: Personel/crLogCompanies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status")] crLogCompany crLogCompany)
        {
            if (ModelState.IsValid && InputValidation(crLogCompany))
            {
                db.crLogCompanies.Add(crLogCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogCompany.Status);
            return View(crLogCompany);
        }

        // GET: Personel/crLogCompanies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCompany crLogCompany = db.crLogCompanies.Find(id);
            if (crLogCompany == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogCompany.Status);
            return View(crLogCompany);
        }

        // POST: Personel/crLogCompanies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status")] crLogCompany crLogCompany)
        {
            if (ModelState.IsValid && InputValidation(crLogCompany))
            {
                db.Entry(crLogCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogCompany.Status);
            return View(crLogCompany);
        }

        // GET: Personel/crLogCompanies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCompany crLogCompany = db.crLogCompanies.Find(id);
            if (crLogCompany == null)
            {
                return HttpNotFound();
            }
            return View(crLogCompany);
        }

        // POST: Personel/crLogCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogCompany crLogCompany = db.crLogCompanies.Find(id);
            db.crLogCompanies.Remove(crLogCompany);
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



        public bool InputValidation(crLogCompany crLogCompany)
        {
            bool isValid = true;

            if (crLogCompany.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }


            return isValid;
        }
    }
}
