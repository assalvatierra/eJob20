using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Controllers
{
    public class InvItemCommisController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        private List<SelectListItem> SourceList = new List<SelectListItem> {
                new SelectListItem { Value = "Motor OiL", Text = "Motor Oil" },
                new SelectListItem { Value = "Gear Oil", Text = "Gear Oil" },
                new SelectListItem { Value = "Others", Text = "Others" },
                };


        // GET: InvItemCommis/{id}
        public ActionResult Index(int? id)
        {
            if (id == null)
                return RedirectToAction("PageNotFound", "Home",null);

            var invItemCommis = db.InvItemCommis.Where(i=>i.InvItemId == id);

            ViewBag.ItemDesc = db.InvItems.Find(id).Description;
            ViewBag.InvItemId = id;

            return View(invItemCommis.ToList());
        }


        // GET: InvItemCommis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItemCommi invItemCommi = db.InvItemCommis.Find(id);
            if (invItemCommi == null)
            {
                return HttpNotFound();
            }

            ViewBag.InvItemId = invItemCommi.InvItemId;
            return View(invItemCommi);
        }

        // GET: InvItemCommis/Create
        public ActionResult Create(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvItemCommi itemCommi = new InvItemCommi();

            itemCommi.InvItemId = (int)id;
            itemCommi.Unit = "per Liter";

            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", id);
            ViewBag.Source = new SelectList(SourceList, "Value", "Text");
            ViewBag.ItemId = id;

            return View(itemCommi);
        }

        // POST: InvItemCommis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount,Unit,Source,InvItemId")] InvItemCommi invItemCommi)
        {
            if (ModelState.IsValid && InputValidation(invItemCommi))
            {
                db.InvItemCommis.Add(invItemCommi);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = invItemCommi.InvItemId });
            }

            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invItemCommi.InvItemId);
            ViewBag.Source = new SelectList(SourceList, "Value", "Text", invItemCommi.Source);
            ViewBag.ItemId = invItemCommi.InvItemId;

            return View(invItemCommi);
        }

        // GET: InvItemCommis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItemCommi invItemCommi = db.InvItemCommis.Find(id);
            if (invItemCommi == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invItemCommi.InvItemId);
            ViewBag.Source = new SelectList(SourceList, "Value", "Text", invItemCommi.Source);
            ViewBag.ItemId = invItemCommi.InvItemId;

            return View(invItemCommi);
        }

        // POST: InvItemCommis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount,Unit,Source,InvItemId")] InvItemCommi invItemCommi)
        {
            if (ModelState.IsValid && InputValidation(invItemCommi))
            {
                db.Entry(invItemCommi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = invItemCommi.InvItemId });
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invItemCommi.InvItemId);
            ViewBag.Source = new SelectList(SourceList, "Value", "Text", invItemCommi.Source);
            ViewBag.ItemId = invItemCommi.InvItemId;

            return View(invItemCommi);
        }

        // GET: InvItemCommis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItemCommi invItemCommi = db.InvItemCommis.Find(id);
            if (invItemCommi == null)
            {
                return HttpNotFound();
            }

            ViewBag.ItemId = invItemCommi.InvItemId;
            return View(invItemCommi);
        }

        // POST: InvItemCommis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvItemCommi invItemCommi = db.InvItemCommis.Find(id);
            db.InvItemCommis.Remove(invItemCommi);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = invItemCommi.InvItemId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool InputValidation(InvItemCommi invItemCommi)
        {
            bool isValid = true;

            if (invItemCommi.Amount < 0)
            {
                ModelState.AddModelError("Amount", "Invalid Amount");
                isValid = false;
            }

            if (invItemCommi.Unit.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Unit", "Invalid Unit");
                isValid = false;
            }

            if (invItemCommi.Source.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Source", "Invalid Source");
                isValid = false;
            }



            return isValid;
        }

    }
}
