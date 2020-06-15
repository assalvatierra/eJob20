using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class VehicleModelsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: AutoCare/VehicleModels
        public ActionResult Index()
        {
            var vehicleModels = db.VehicleModels.Include(v => v.VehicleBrand).Include(v => v.VehicleType).Include(v => v.VehicleTransmission).Include(v => v.VehicleFuel);
            return View(vehicleModels.ToList());
        }

        // GET: AutoCare/VehicleModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // GET: AutoCare/VehicleModels/Create
        public ActionResult Create()
        {
            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Brand");
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type");
            ViewBag.VehicleTransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Type");
            ViewBag.VehicleFuelId = new SelectList(db.VehicleFuels, "Id", "Fuel");
            return View();
        }

        // POST: AutoCare/VehicleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Make,Variant,VehicleBrandId,VehicleTypeId,Remarks,VehicleTransmissionId,VehicleFuelId")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                db.VehicleModels.Add(vehicleModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Brand", vehicleModel.VehicleBrandId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicleModel.VehicleTypeId);
            ViewBag.VehicleTransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Type", vehicleModel.VehicleTransmissionId);
            ViewBag.VehicleFuelId = new SelectList(db.VehicleFuels, "Id", "Fuel", vehicleModel.VehicleFuelId);
            return View(vehicleModel);
        }

        // GET: AutoCare/VehicleModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Brand", vehicleModel.VehicleBrandId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicleModel.VehicleTypeId);
            ViewBag.VehicleTransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Type", vehicleModel.VehicleTransmissionId);
            ViewBag.VehicleFuelId = new SelectList(db.VehicleFuels, "Id", "Fuel", vehicleModel.VehicleFuelId);
            return View(vehicleModel);
        }

        // POST: AutoCare/VehicleModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Make,Variant,VehicleBrandId,VehicleTypeId,Remarks,VehicleTransmissionId,VehicleFuelId")] VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicleModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Brand", vehicleModel.VehicleBrandId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Type", vehicleModel.VehicleTypeId);
            ViewBag.VehicleTransmissionId = new SelectList(db.VehicleTransmissions, "Id", "Type", vehicleModel.VehicleTransmissionId);
            ViewBag.VehicleFuelId = new SelectList(db.VehicleFuels, "Id", "Fuel", vehicleModel.VehicleFuelId);
            return View(vehicleModel);
        }

        // GET: AutoCare/VehicleModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // POST: AutoCare/VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            db.VehicleModels.Remove(vehicleModel);
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
