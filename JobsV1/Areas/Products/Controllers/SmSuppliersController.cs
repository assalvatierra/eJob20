using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Products.Models;
using Newtonsoft.Json;

namespace JobsV1.Areas.Products.Controllers
{
    public class SmSuppliersController : Controller
    {
        private ProdDBContainer db = new ProdDBContainer();
       

        // GET: Products/SmSuppliers
        public ActionResult Index()
        {
            return View(db.SmSuppliers.ToList());
        }

        // GET: Products/SmSuppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmSupplier smSupplier = db.SmSuppliers.Find(id);
            if (smSupplier == null)
            {
                return HttpNotFound();
            }

            PartialView_Info((int)id);

            return View(smSupplier);
        }

        //GET: Ajax - Table Result 
        //Get the list of suppliers
        [HttpGet]
        public string TableResult(string search)
        {
            SupplierClass sup = new SupplierClass();
           
            //convert list to json object
            return JsonConvert.SerializeObject(sup.getSuppliers(search), Formatting.Indented);
        }

        // GET: Products/SmSuppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/SmSuppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Remarks")] SmSupplier smSupplier)
        {
            if (ModelState.IsValid)
            {
                db.SmSuppliers.Add(smSupplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smSupplier);
        }

        // GET: Products/SmSuppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmSupplier smSupplier = db.SmSuppliers.Find(id);
            if (smSupplier == null)
            {
                return HttpNotFound();
            }
            return View(smSupplier);
        }

        // POST: Products/SmSuppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Remarks")] SmSupplier smSupplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smSupplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smSupplier);
        }

        // GET: Products/SmSuppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmSupplier smSupplier = db.SmSuppliers.Find(id);
            if (smSupplier == null)
            {
                return HttpNotFound();
            }
            return View(smSupplier);
        }

        // POST: Products/SmSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmSupplier smSupplier = db.SmSuppliers.Find(id);
            db.SmSuppliers.Remove(smSupplier);
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
        
        #region Supplier Info
        public void PartialView_Info(int id)
        {
            ViewBag.supInfo = db.SmSupplierInfoes.Where(s => s.SmSupplierId == id).ToList();

        }

        [HttpPost]
        public ActionResult AddSupInfo(int supId, string infolabel, string infoValue, string infoRemarks)
        {
            //Search string filter
            if (!string.IsNullOrWhiteSpace(infolabel) || !string.IsNullOrEmpty(infolabel))
            {
                SmSupplierInfo info = new SmSupplierInfo();
                info.Label = infolabel;
                info.Value = infoValue;
                info.Remarks = infoRemarks;
                info.SmSupplierId = supId;

                db.SmSupplierInfoes.Add(info);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = supId });
        }

        [HttpPost]
        public ActionResult EditSupInfo(int Id, int supId, string infolabel, string infoValue, string infoRemarks)
        {
            //Search string filter
            if (!string.IsNullOrWhiteSpace(infolabel) || !string.IsNullOrEmpty(infolabel))
            {
                SmSupplierInfo info = db.SmSupplierInfoes.Find(Id);
                info.Label = infolabel;
                info.Value = infoValue;
                info.Remarks = infoRemarks;
                info.SmSupplierId = supId;

                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = supId });
        }

        //Remove product Info
        public ActionResult RemoveSupInfo(int id)
        {
            SmSupplierInfo supInfo = db.SmSupplierInfoes.Find(id);
            int supId = supInfo.SmSupplierId;
            db.SmSupplierInfoes.Remove(supInfo);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = supId }); //view in personnel details
        }
        #endregion
    }
}
