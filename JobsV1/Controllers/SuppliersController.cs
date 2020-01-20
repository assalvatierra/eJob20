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
        private DateClass dt = new DateClass();
        private SupplierClass supdb = new SupplierClass();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" },
                new SelectListItem { Value = "ACC", Text = "Accredited" },
                new SelectListItem { Value = "AOP", Text = "Acc. On Process" }
                };

        // GET: Suppliers

        public ActionResult Index()
        {
            
            return View(db.Suppliers.ToList());

        }
        
        //Ajax - Table Result 
        //Get the list of suppliers
        public string TableResult(string search, string category, string status, string sort)
        {
            //get supplier list
            List<cSupplierItems> supList = supdb.getSupplierList(search, category,status,sort);
            
            //convert list to json object
            return JsonConvert.SerializeObject(supList, Formatting.Indented);
        }

        //Ajax - Table Result 
        //Get the list of suppliers
        public string TableResultProducts(string search, string category, string status, string sort)
        {
            //get supplier list
            List<cProductList> prodList = supdb.getProductList(search, category, status, "LOWEST-PRICE");

            //convert list to json object
            return JsonConvert.SerializeObject(prodList, Formatting.Indented);
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

            var supDocListId = db.SupplierDocuments.Where(s => s.SupplierId == id).Select(s=>s.SupDocumentId).ToList();
            
            //current added documents
            var supDocList = db.SupDocuments.Where(s=> supDocListId.Contains(s.Id)).ToList();

            var completeDocs = db.SupDocuments.ToList();

            //get documents not added
           IEnumerable<SupDocument> doclist = completeDocs.Except(supDocList);
            ViewBag.Documents = doclist;
            ViewBag.SupplierId = id;
            ViewBag.supContacts = supplier.SupplierContacts.ToList();
            ViewBag.contactStatus = db.SupplierContactStatus.ToList();
            ViewBag.supDocuments = db.SupplierDocuments.Where(s => s.SupplierId == id).Include(s=>s.SupDocument);

            InvItemsPartial((int)id);

            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status,Address,CountryId,Website,Code")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();

                return RedirectToAction("CreateSupContactForm", new { id = supplier.Id });
                //return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            //ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryName);
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            //return View(supplier);
            return RedirectToAction("Details", new { id = supplier.Id });
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
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryId);
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }
       

       // POST: Suppliers/Edit/5
       // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
       // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status,Address,CountryId,Website,Code")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Suppliers", new { id = supplier.Id });
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", supplier.CountryId);

            return RedirectToAction("Details", "Suppliers", new { id = supplier.Id });
            //return View(supplier);
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
        //items of suppliers View
        public ActionResult InvItems(int id)
        {
            InvItemsPartial((int)id);
            ViewBag.Id = id;

            return View(db.SupplierInvItems.Where(s=>s.SupplierId == id).ToList());
        }


        public void InvItemsPartial(int id)
        {
            ViewBag.SupplierId = id;
            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.UnitList = db.SupplierUnits.ToList();

            //get items not added
            var allitems = db.InvItems.ToList();
            //get list of ids of items of supplier
            var supItemsIds = db.SupplierInvItems.Where(d => d.SupplierId == id).ToList().Select(d => d.InvItemId);
            //get list of items of supplier
            var supItems = db.InvItems.Where(d=> supItemsIds.Contains(d.Id)).ToList();
            //display items except existing 
            var filteredItems = allitems.Except(supItems).ToList();
            
            ViewBag.ItemList = filteredItems;

            ViewBag.InvItems = db.SupplierInvItems.Where(s => s.SupplierId == id).ToList();
        }

        //POST: /Suppliers/AddInvItems
        //add new items to the supplier
        public ActionResult AddInvItems(int InvID, int supID) {
            db.SupplierInvItems.Add(new SupplierInvItem {
                InvItemId = InvID,
                SupplierId = supID
            });
            db.SaveChanges();

            return RedirectToAction("Details", "Suppliers", new { id = supID });
        }

        //POST: /Suppliers/AddInvItems
        //remove item(s) from the supplier
        public ActionResult RemoveInvItems(int id)
        {
            SupplierInvItem tempitem = db.SupplierInvItems.Find(id);
            
            //check and remove existing items
            foreach (var itemRate in tempitem.SupplierItemRates.ToList())
            {
                RemoveRateInvItem(itemRate.Id);
            }
            
            SupplierInvItem item = db.SupplierInvItems.Find(id);

            db.SupplierInvItems.Remove(item);
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = item.SupplierId });
        }

        #endregion

        #region invItemRate

        public ActionResult AddRateInvItems(int id, string Particulars, string Material,string Rate, int Unit, string Remarks, string TradeTerm, string Tolerance, string ValidFrom, string ValidTo, string By ,string ProcBy )
        {
            db.SupplierItemRates.Add(new SupplierItemRate
            {
                Particulars = Particulars,
                Material = Material,
                ItemRate = Rate,
                SupplierUnitId = Unit,
                Remarks = Remarks,
                TradeTerm = TradeTerm,
                Tolerance = Tolerance,
                DtValidFrom = ValidFrom,
                DtValidTo = ValidTo,
                SupplierInvItemId = id,
                By = By,
                ProcBy = ProcBy
            });
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = id });
        }

        public ActionResult EditRateInvItems(int id, string Particulars, string Material, string Rate, int Unit, string ValidFrom, string ValidTo, string Remarks, string TradeTerm, string Tolerance, int SupInvId, string By, string ProcBy)
        {
         
            SupplierItemRate itemRate = db.SupplierItemRates.Find(id);
            itemRate.Particulars = Particulars;
            itemRate.Material = Material;
            itemRate.ItemRate = Rate;
            itemRate.SupplierUnitId = Unit;
            itemRate.TradeTerm = TradeTerm;
            itemRate.Tolerance = Tolerance;
            itemRate.Remarks = Remarks;
            itemRate.DtValidFrom = ValidFrom;
            itemRate.DtValidTo = ValidTo;
            itemRate.SupplierInvItemId = SupInvId;
            itemRate.By = By;
            itemRate.ProcBy = ProcBy;

            db.Entry(itemRate).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = id });
        }

        public void RemoveRateInvItem(int id)
        {
            SupplierItemRate itemRate = db.SupplierItemRates.Find(id);
            db.SupplierItemRates.Remove(itemRate);
            db.SaveChanges();
        }

        #endregion

        #region DeactivateList

        //get list of customers with
        //a year past jobs and no recent jobs 
        public ActionResult DeActivateSupplierList()
        {
            //get supplier list
            List<Supplier> suppliers = supdb.getDeactivatedList();
            
            return View(suppliers);
        }
        
        //Deactive a multiple customer by changing its 
        //status from ACT to INC. 
        public ActionResult DeactivateAll()
        {
            //get supplier list
            List<Supplier> suppliers = supdb.getDeactivatedList();

            foreach (var sup in suppliers)
            {
                sup.Status = "INC";    //deactivate customer
                db.Entry(sup).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeActivateSupplierList", "Suppliers");
        }
        
        //Deactive a single customer by changing its 
        //status from ACT to INC
        public ActionResult DeactivateSingle(int id)
        {
            var supplier = db.Suppliers.Find(id);
            if (supplier != null)
            {
                supplier.Status = "INC";    //deactivate customer
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("DeactivateSupplierList", "Suppliers");
        }

        #endregion

        #region SupplierContact

        //  Create new Supplier contact
        public ActionResult CreateSupContactForm(int id)
        {
            ViewBag.SupplierId = id;
            ViewBag.contactStatus = db.SupplierContactStatus.ToList();

            return View();
        }

        //  Create new Supplier contact
        public ActionResult CreateSupContact(int SupplierId, string  Name, string Mobile, string Landline, string SkypeId, string ViberId , string WhatsApp, string Email, int Status, string Remarks, string WeChat, string Position, string Department)
        {
            SupplierContact supContact = new SupplierContact();
            supContact.SupplierId = SupplierId;
            supContact.Name = Name;
            supContact.Mobile = Mobile;
            supContact.Landline = Landline;
            supContact.SkypeId = SkypeId;
            supContact.ViberId = ViberId;
            supContact.Remarks = Remarks;
            supContact.WhatsApp = WhatsApp;
            supContact.Email = Email;
            supContact.SupplierContactStatusId = Status;
            supContact.WeChat = WeChat;
            supContact.Position = Position;
            supContact.Department = Position;

            if (SupplierId != 0)
            {
               db.SupplierContacts.Add(supContact);
               db.SaveChanges();
            }
            return RedirectToAction("Details" , new { id = SupplierId });
           
        }

        //  Create new Supplier contact
        public ActionResult EditSupContact(int id, string Name, string Mobile, string Landline, string SkypeId, string ViberId, string Remarks, string WhatsApp, string Email, int Status, string WeChat, string Position, string Department)
        {
            SupplierContact supContact = db.SupplierContacts.Find(id);
            supContact.Name = Name;
            supContact.Mobile = Mobile;
            supContact.Landline = Landline;
            supContact.SkypeId = SkypeId;
            supContact.ViberId = ViberId;
            supContact.Remarks = Remarks;
            supContact.WhatsApp = WhatsApp;
            supContact.Email = Email;
            supContact.SupplierContactStatusId = Status;
            supContact.WeChat = WeChat;
            supContact.Position = Position;
            supContact.Department = Department;

            db.Entry(supContact).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = supContact });
        }

        public ActionResult deleteSupContact(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierContact supContact = db.SupplierContacts.Find(id);
            if (supContact == null)
            {
                return HttpNotFound();
            }
            int tempId = supContact.SupplierId;

            db.SupplierContacts.Remove(supContact);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = tempId });

        }

        #endregion

        #region Documents

        //Add Document to the Supplier Document List
        public ActionResult AddDocuments(int docId, int supId)
        {
            try
            {
                db.SupplierDocuments.Add(new SupplierDocument
                {
                    SupDocumentId = docId,
                    SupplierId = supId
                });
                db.SaveChanges();

            }
            catch (Exception ex)
            { }

            return RedirectToAction("Details", "Suppliers", new { id = supId });
        }

        public ActionResult RemoveSupDocument(int id)
        {

            SupplierDocument supplierDoc = db.SupplierDocuments.Find(id);
            var supplierId = supplierDoc.SupplierId;
            db.SupplierDocuments.Remove(supplierDoc);
            db.SaveChanges();

            return RedirectToAction("Details", "Suppliers", new { id = supplierId });

        }

        #endregion
    }
}
