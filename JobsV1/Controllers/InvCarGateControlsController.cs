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
    public class InvCarGateControlsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: InvCarGateControls
        public ActionResult Index()
        {
            var invCarGateControls = db.InvCarGateControls.Include(i => i.InvItem).OrderByDescending(c=>c.dtControl);

            //get records past their next odometer & schedule change
            //odometer
            List<InvCarRecord> priority = new List<InvCarRecord>();

            foreach (var carList in db.InvItems.Where(s => s.ViewLabel == "UNIT").ToList())
            {
                //latest next odometer of the car
                var priorityRecords = db.InvCarRecords.Include(i => i.InvCarRecordType).Include(i => i.InvItem)
                    .Where(c => c.InvItemId == carList.Id).OrderByDescending(s => s.NextOdometer).FirstOrDefault();

                priority.Add(priorityRecords);
            }

            ViewBag.priority = priority;
            
            return View(invCarGateControls.ToList());
        }

        // GET: InvCarGateControls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarGateControl invCarGateControl = db.InvCarGateControls.Find(id);
            if (invCarGateControl == null)
            {
                return HttpNotFound();
            }
            return View(invCarGateControl);
        }

        // GET: InvCarGateControls/Create
        public ActionResult Create(int? control)
        {
            InvCarGateControl invcar = new InvCarGateControl();

            if (control == null)
            {
                invcar.In_Out_flag = 1; //gate out
            }
            else
            {
                invcar.In_Out_flag = (int)control;
            }
            
            invcar.dtControl = DateTime.Now;

            var invItems = db.InvItems.Where(s => s.ViewLabel == "UNIT").Select(
                    s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.ItemCode.ToString() + " - " + s.Description
                    }
                );
            ViewBag.recordType = invcar.In_Out_flag == 1 ? "Out" : "In";
            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text");
            return View(invcar);
        }

        // POST: InvCarGateControls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvItemId,In_Out_flag,Odometer,dtControl,Remarks,Driver,Inspector")] InvCarGateControl invCarGateControl)
        {
            if (ModelState.IsValid)
            {
                db.InvCarGateControls.Add(invCarGateControl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarGateControl.InvItemId);
            return View(invCarGateControl);
        }

        // GET: InvCarGateControls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarGateControl invCarGateControl = db.InvCarGateControls.Find(id);
            if (invCarGateControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarGateControl.InvItemId);
            return View(invCarGateControl);
        }

        // POST: InvCarGateControls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvItemId,In_Out_flag,Odometer,dtControl,Remarks,Driver,Inspector")] InvCarGateControl invCarGateControl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invCarGateControl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarGateControl.InvItemId);
            return View(invCarGateControl);
        }

        // GET: InvCarGateControls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarGateControl invCarGateControl = db.InvCarGateControls.Find(id);
            if (invCarGateControl == null)
            {
                return HttpNotFound();
            }
            return View(invCarGateControl);
        }

        // POST: InvCarGateControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvCarGateControl invCarGateControl = db.InvCarGateControls.Find(id);
            db.InvCarGateControls.Remove(invCarGateControl);
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
