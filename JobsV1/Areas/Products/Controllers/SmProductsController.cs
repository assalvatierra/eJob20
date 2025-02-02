﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Products.Models;
using JobsV1.Models;
using Newtonsoft.Json;


namespace JobsV1.Areas.Products.Controllers
{

    public class SmProductsController : Controller
    {
        private ProdDBContainer db = new ProdDBContainer();

        // GET: Products/SmProducts
        public ActionResult Index()
        {
            var smProducts = db.SmProducts.Include(s => s.SmProdStatu).Include(s => s.SmBranch);
            return View(smProducts.ToList().OrderBy(s=>s.ValidStart));
        }

        // GET: Products/SmProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProduct smProduct = db.SmProducts.Find(id);
            if (smProduct == null)
            {
                return HttpNotFound();
            }

            PartialView_Desc((int)id);
            PartialView_Info((int)id);
            PartialView_Cat((int)id);
            PartialView_ProdSup((int)id);
            PartialView_Rates((int)id);
            PartialView_File((int)id);

            return View(smProduct);
        }

        //GET : Ajax - Table Result 
        //Get the list of products
        [HttpGet]
        public string TableResult(string search, string status)
        {
            ProductClass prod = new ProductClass();
            
            var tableContent = prod.getProductList(search, status);

            //convert list to json object
            return JsonConvert.SerializeObject(tableContent, Formatting.Indented);
        }

        // GET: Products/SmProducts/Create
        public ActionResult Create()
        {
            //new date
            DateClass dateNow = new DateClass();

            SmProduct prod = new SmProduct();
            prod.BranchId = 1;
            prod.ProdStatusId = 1;
            prod.ValidStart = dateNow.GetCurrentDate();
            prod.ValidEnd = dateNow.GetCurrentDate().AddYears(1);

            ViewBag.SmProdStatusId = new SelectList(db.SmProdStatus, "Id", "Status");
            ViewBag.SmBranchId = new SelectList(db.SmBranches, "Id", "Name");
            return View(prod);
        }

        // POST: Products/SmProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SmBranchId,Code,Name,Remarks,BranchId,ProdStatusId,ValidStart,ValidEnd,Price,Contracted,SmProdStatusId")] SmProduct smProduct)
        {
            if (ModelState.IsValid)
            {
                db.SmProducts.Add(smProduct);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", new { id = smProduct.Id });
            }

            ViewBag.SmProdStatusId = new SelectList(db.SmProdStatus, "Id", "Status", smProduct.SmProdStatusId);
            ViewBag.SmBranchId = new SelectList(db.SmBranches, "Id", "Name", smProduct.SmBranchId);
            return View(smProduct);
        }

        // GET: Products/SmProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProduct smProduct = db.SmProducts.Find(id);
            if (smProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.SmProdStatusId = new SelectList(db.SmProdStatus, "Id", "Status", smProduct.SmProdStatusId);
            ViewBag.SmBranchId = new SelectList(db.SmBranches, "Id", "Name", smProduct.SmBranchId);
            return View(smProduct);
        }

        // POST: Products/SmProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SmBranchId,Code,Name,Remarks,BranchId,ProdStatusId,ValidStart,ValidEnd,Price,Contracted,SmProdStatusId")] SmProduct smProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = smProduct.Id});
            }
            ViewBag.SmProdStatusId = new SelectList(db.SmProdStatus, "Id", "Status", smProduct.SmProdStatusId);
            ViewBag.SmBranchId = new SelectList(db.SmBranches, "Id", "Name", smProduct.SmBranchId);
            return View(smProduct);
        }

        // GET: Products/SmProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProduct smProduct = db.SmProducts.Find(id);
            if (smProduct == null)
            {
                return HttpNotFound();
            }
            return View(smProduct);
        }

        // POST: Products/SmProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmProduct smProduct = db.SmProducts.Find(id);
            smProduct.SmProdStatusId = 2;
            db.Entry(smProduct).State = EntityState.Modified;
            
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

        public ActionResult Deactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProduct smProduct = db.SmProducts.Find(id);
            if (smProduct == null)
            {
                return HttpNotFound();
            }

            //change status
            smProduct.SmProdStatusId = 2;
            db.Entry(smProduct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        #region Product Description
        public void PartialView_Desc(int id)
        {
            ViewBag.prodDesc = db.SmProdDescs.Where(s=>s.SmProductId == id).OrderBy(s=>s.SortNo).ToList();
            ViewBag.prodDescList = db.SmProdDescs.ToList();
        }

        //add product description
        public ActionResult AddDesc(int prodId,int sort,string text)
        {
            //Search string filter
            if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrEmpty(text))
            {
                SmProdDesc desc = new SmProdDesc();
                desc.Description = text;
                desc.SmProductId = prodId;
                desc.SortNo = sort;
                
                db.SmProdDescs.Add(desc);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = prodId });
        }

        //edit product description
        public ActionResult EditDesc(int prodId, int descId, int sort, string text)
        {
            //Search string filter
            if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrEmpty(text))
            {
                SmProdDesc desc = db.SmProdDescs.Find(descId);
                desc.Description = text;
                desc.SmProductId = prodId;
                desc.SortNo = sort;

                db.Entry(desc).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = prodId });
        }

        //edit product description
        [HttpGet]
        public string getDesc(int Id)
        {
            SmProdDesc prodDesc = db.SmProdDescs.Find(Id);

            productDesc desc = new productDesc();
            desc.Id   = prodDesc.Id;
            desc.Desc = prodDesc.Description;
            desc.Sort = prodDesc.SortNo;

            return JsonConvert.SerializeObject(desc, Formatting.Indented);
        }

        //remove product description
        public ActionResult RemoveDesc(int id)
        {
            SmProdDesc prodDesc = db.SmProdDescs.Find(id);
            int prodId = prodDesc.SmProductId;
            db.SmProdDescs.Remove(prodDesc);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion
        
        #region Product Info
        public void PartialView_Info(int id)
        {
            ViewBag.prodInfo = db.SmProdInfoes.Where(s => s.SmProductId == id).ToList();
        }
        
        public ActionResult AddInfo(int prodId, string infolabel, string infoValue, string infoRemarks)
        {
            //Search string filter
            if (!string.IsNullOrWhiteSpace(infolabel) || !string.IsNullOrEmpty(infolabel) )
            {
                SmProdInfo info = new SmProdInfo();
                info.Label = infolabel;
                info.Value = infoValue;
                info.Remarks = infoRemarks;
                info.SmProductId = prodId;
               
                db.SmProdInfoes.Add(info);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = prodId });
        }

        //Edit product description
        public ActionResult EditInfo(int? id,int prodId, string infolabel, string infoValue, string infoRemarks)
        {
            if (!string.IsNullOrWhiteSpace(infolabel) || !string.IsNullOrEmpty(infolabel))
            {
                if (id != null)
                {

                SmProdInfo info = db.SmProdInfoes.Find(id);
                info.Label = infolabel;
                info.Value = infoValue;
                info.Remarks = infoRemarks;
                //info.SmProductId = prodId;

                db.Entry(info).State = EntityState.Modified;
                db.SaveChanges();
                }
            }
            
            return RedirectToAction("Details", new { id = prodId });
        }

        //Remove product Info
        public ActionResult RemoveInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProdInfo prodInfo = db.SmProdInfoes.Find(id);
                int prodId = prodInfo.SmProductId;
                db.SmProdInfoes.Remove(prodInfo);
                db.SaveChanges();
            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion

        #region Product Category

        public void PartialView_Cat(int id)
        {
            ViewBag.prodCat = db.SmProdCats.Where(s => s.SmProductId == id).Include(s=>s.SmCategory).ToList();
            ViewBag.prodCatList = db.SmCategories.ToList();

        }

        //add product category
        public ActionResult AddCat(int prodId, int catId)
        {
            SmProdCat cat = new SmProdCat();
            cat.SmCategoryId = catId;
            cat.SmProductId = prodId;

            db.SmProdCats.Add(cat);
            db.SaveChanges();
            
            return RedirectToAction("Details", new { id = prodId });
        }

        //Remove product category
        public ActionResult RemoveCat(int id)
        {
            SmProdCat prodCat = db.SmProdCats.Find(id);
            int prodId = prodCat.SmProductId;
            db.SmProdCats.Remove(prodCat);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion

        #region Product Supplier
        public void PartialView_ProdSup(int id)
        {
            ViewBag.prodSup = db.SmProdSuppliers.Where(s => s.SmProductId == id).ToList();
            ViewBag.supList = db.SmSuppliers.ToList();
        }

        //add product supplier
        public ActionResult AddProdSup(int prodId, int supId, string startdate, string enddate,
            string price,string contracted)
        {
           
            SmProdSupplier prodSup = new SmProdSupplier();
            prodSup.SmProductId = prodId;
            prodSup.SmSupplierId = supId;
            prodSup.ValidStart = DateTime.Parse(startdate);
            prodSup.ValidEnd = DateTime.Parse(enddate);
            prodSup.Price = Decimal.Parse(price);
            prodSup.Contracted = decimal.Parse(contracted);
            
            db.SmProdSuppliers.Add(prodSup);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId });
        }

        //add product supplier
        public ActionResult EditProdSup(int Id,int prodId, int supId, string startdate, string enddate,
            string price, string contracted)
        {
            SmProdSupplier prodSup = db.SmProdSuppliers.Find(Id);
            prodSup.SmProductId = prodId;
            prodSup.SmSupplierId = supId;
            prodSup.ValidStart = DateTime.Parse(startdate);
            prodSup.ValidEnd = DateTime.Parse(enddate);
            prodSup.Price = Decimal.Parse(price);
            prodSup.Contracted = decimal.Parse(contracted);

            db.Entry(prodSup).State = EntityState.Modified;
            db.SaveChanges();
            
            return RedirectToAction("Details", new { id = prodId });
        }

        //Remove product category
        public ActionResult RemoveProdSup(int id)
        {
            SmProdSupplier prodSup = db.SmProdSuppliers.Find(id);
            int prodId = prodSup.SmProductId;
            db.SmProdSuppliers.Remove(prodSup);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion
        
        #region Product Rates
        public void PartialView_Rates(int id)
        {
            ViewBag.prodrates = db.SmRates.Where(s => s.SmProductId == id).ToList();
            ViewBag.rateUOMList = db.SmRateUoMs.ToList();
        }

        //add new product rate
        public ActionResult AddProdRate(int prodId, int qty, int? uomId, decimal rate, decimal  drate )
        {
            SmRate prodRate      = new SmRate();
            prodRate.SmProductId = prodId;
            prodRate.Qty         = qty;
            prodRate.SmRateUoMId = (int)uomId;
            prodRate.Rate        = rate;
            prodRate.DRate       = drate;
            
            db.SmRates.Add(prodRate);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId });
        }

        //edit product rate
        public ActionResult EditProdRate(int? id, int prodId, int qty, int? uomId, decimal rate, decimal drate)
        {
            if (id != null)
            {
                SmRate prodRate = db.SmRates.Find(id);
                prodRate.Qty = qty;
                prodRate.SmRateUoMId = (int)uomId;
                prodRate.Rate = rate;
                prodRate.DRate = drate;
            
                db.Entry(prodRate).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = prodId });
        }

        //Remove product rate
        public ActionResult RemoveProdRate(int id)
        {
            SmRate prodRate = db.SmRates.Find(id);
            int prodId = prodRate.SmProductId;
            db.SmRates.Remove(prodRate);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion

        #region Product Files
        public void PartialView_File(int id)
        {
            ViewBag.files = db.SmFiles.Where(s => s.SmProductId == id).ToList();
        }

        //add product category
        public ActionResult AddFile(int prodId, string desc,string filePath)
        {
            SmFile file = new SmFile();
            file.Desc = desc;
            file.Link = filePath;
            file.SmProductId = prodId;

            db.SmFiles.Add(file);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId });
        }

        //Remove product category
        public ActionResult RemoveFile(int id)
        {
            SmFile file = db.SmFiles.Find(id);
            int prodId = file.SmProductId;
            db.SmFiles.Remove(file);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = prodId }); //view in personnel details
        }
        #endregion

        #region ads

        #endregion
    }
}
