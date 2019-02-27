using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class CustCatsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CustCats
        public ActionResult Index()
        {
            var custCats = db.CustCats.Include(c => c.Customer).Include(c => c.CustCategory);
            return View(custCats.ToList());
        }

        // GET: CustCats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCat custCat = db.CustCats.Find(id);
            if (custCat == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // GET: CustCats/Create
        public ActionResult Create(int? id)
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", id);
            ViewBag.CustCategoryId = new SelectList(db.CustCategories, "Id", "Name");
            return View();
            // return RedirectToAction("Details", "Customers", new { id = id });
        }

        // POST: CustCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,CustCategoryId")] CustCat custCat)
        {
            if (ModelState.IsValid)
            {
                db.CustCats.Add(custCat);
                db.SaveChanges();
                //  return RedirectToAction("Index");
                return RedirectToAction("Details", "Customers", new { id = custCat.CustomerId });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custCat.CustomerId);
            ViewBag.CustCategoryId = new SelectList(db.CustCategories, "Id", "Name", custCat.CustCategoryId);
            return View(custCat);
        }

        // GET: CustCats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCat custCat = db.CustCats.Find(id);
            if (custCat == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custCat.CustomerId);
            ViewBag.CustCategoryId = new SelectList(db.CustCategories, "Id", "Name", custCat.CustCategoryId);
            return View(custCat);
        }

        // POST: CustCats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerId,CustCategoryId")] CustCat custCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custCat.CustomerId);
            ViewBag.CustCategoryId = new SelectList(db.CustCategories, "Id", "Name", custCat.CustCategoryId);
            return View(custCat);
        }

        // GET: CustCats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCat custCat = db.CustCats.Find(id);
            if (custCat == null)
            {
                return HttpNotFound();
            }
            return View(custCat);
        }

        // POST: CustCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustCat custCat = db.CustCats.Find(id);
            db.CustCats.Remove(custCat);
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


        // POST: CustCats/Delete/catid?custid?
        public ActionResult Remove(int? catid, int custid)
        {
            CustCat cust = db.CustCats.Where(c => c.CustCategoryId == catid && c.CustomerId == custid).FirstOrDefault();
            CustCat custCat = db.CustCats.Find(cust.Id);
            db.CustCats.Remove(custCat);
            db.SaveChanges();
            return RedirectToAction("Details", "Customers", new { id = custid });
        }


        public ActionResult addCategory(int catid, int userid)
        {
            
                CustCat cat = new CustCat();

                db.CustCats.Add(new CustCat
                {
                    CustCategoryId = catid,
                    CustomerId = userid
                });
                db.SaveChanges();
                //  return RedirectToAction("Index");
                return RedirectToAction("Details", "Customers", new { id =userid });
        
        }

    }
}
