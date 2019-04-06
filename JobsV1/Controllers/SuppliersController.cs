using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class SuppliersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        // GET: Suppliers

        class SupplierList
        {
            public int    Id       { get; set; }
            public string Name     { get; set; }
            public string Contact1 { get; set; }
            public string Contact2 { get; set; }
            public string Contact3 { get; set; }
            public string Email    { get; set; }
            public string Status   { get; set; }
            public string SupType  { get; set; }
            public string City     { get; set; }
            public string Dtls     { get; set; }
        }

        public ActionResult Index()
        {
            
            return View(db.Suppliers.ToList());
        }
        

        //Ajax - Table Result 
        //Get the list of suppliers
        //containing the search string,
        //if search is empty, return all actve items
        public string TableResult(string search, string status)
        {
            List<Supplier> suppliers = new List<Supplier>();
            List<SupplierList> supList = new List<SupplierList>();
            suppliers = db.Suppliers.ToList();

            //Search string filter
            if (status == "ALL")
            {
                suppliers = suppliers.ToList();
            }
            else
            {
                suppliers = suppliers.Where(s => s.Status == status).ToList();
            }
            
            //Search string filter
            if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            {
                suppliers = suppliers.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            //build temp supplier list
            foreach (var item in suppliers)
            {
                supList.Add(new SupplierList
                {
                    Id       = item.Id,
                    Name     = item.Name,
                    Contact1 = String.IsNullOrEmpty(item.Contact1) ? "--" : item.Contact1,
                    Contact2 = String.IsNullOrEmpty(item.Contact2) ? "--" : item.Contact2,
                    Contact3 = String.IsNullOrEmpty(item.Contact3) ? "--" : item.Contact3,
                    Email    = String.IsNullOrEmpty(item.Contact3) ? "--" : item.Email,
                    Status   = item.Status,
                    City     = item.City.Name,
                    SupType  = item.SupplierType.Description,
                    Dtls     = item.Details 
                });
            }
            
            //convert list to json object
            return JsonConvert.SerializeObject(supList, Formatting.Indented);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text");

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description", supplier.SupplierTypeId);

            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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

        #region InvItems

        public ActionResult InvItems(int id)
        {
            ViewBag.SupplierId = id;
            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.ItemList = db.InvItems.ToList();
            return View(db.SupplierInvItems.Where(s=>s.SupplierId == id).ToList());
        }

        public ActionResult AddInvItems(int InvID, int supID) {
            db.SupplierInvItems.Add(new SupplierInvItem {
                InvItemId = InvID,
                SupplierId = supID
            });
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = supID });
        }


        public ActionResult RemoveInvItems(int id)
        {
            SupplierInvItem item = db.SupplierInvItems.Find(id);
            db.SupplierInvItems.Remove(item);

            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = item.SupplierId });
        }

        #endregion

        #region DiactivateList

        //get list of customers with
        //a year past jobs and no recent jobs 
        public ActionResult DiActivateSupplierList()
        {
            //get supplier list
            List<Supplier> suppliers = getDiactivatedList();
            
            return View(suppliers);
        }
        
        //Diactive a multiple customer by changing its 
        //status from ACT to INC. 
        public ActionResult DiActivateAll()
        {
            //get supplier list
            List<Supplier> suppliers = getDiactivatedList();

            foreach (var sup in suppliers)
            {
                sup.Status = "INC";    //diactivate customer
                db.Entry(sup).State = EntityState.Modified;
                db.SaveChanges();
            }


            return RedirectToAction("DiActivateSupplierList", "Suppliers");
        }

        //Customers on the list are customers with
        //a year past jobs and no recent jobs 
        private List<Supplier> getDiactivatedList()
        {
            List<int> oldJobs = new List<int>();
            var datetoday = GetCurrentTime().Date.AddDays(-360).Date;

            //get all active suppliers
            var actSuppliers = db.Suppliers.Where(s => s.Status == "ACT").ToList();

            foreach (var sup in actSuppliers)
            {
                //get recent jobservice of supplier
                var jobserviceTemp = db.JobServices.Where(j => j.SupplierId == sup.Id).OrderByDescending(s => s.DtStart).FirstOrDefault();
                if (jobserviceTemp != null)
                {
                    DateTime jobdate = (DateTime)jobserviceTemp.DtStart;
                    //check if job is old o more than a year
                    if (jobdate.Date.CompareTo(datetoday.Date) < 0)
                    {
                        oldJobs.Add(jobserviceTemp.Id);
                    }

                }
            }

            //get recent of jobservices 360 days
            var services = db.JobServices.Where(j => oldJobs.Contains(j.Id)).ToList().Select(s => s.SupplierId);

            //get list of suppliers with jobs
            var suppliers = db.Suppliers.Where(s => services.Contains(s.Id)).ToList();

            return suppliers;
        }

        //Diactive a single customer by changing its 
        //status from ACT to INC
        public ActionResult DiactivateSingle(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier != null)
            {
                supplier.Status = "INC";    //diactivate customer
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DiActivateSupplierList", "Suppliers");
        }

        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }
        #endregion
    }
}
